using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Playables;
using Newtonsoft.Json;


public class CharacterDataSaver : MonoBehaviour
{
    [SerializeField] private PlayerDefaultSettings playerDefaultSettings;
    [SerializeField] private FireBaseManager fbManager;
    [SerializeField] private PlayerProgressManager playerProgressManager;
    private PlayerData playerData;
    private string localPath;
    private bool playerDataInFB = false;

    private async void Start()
    {
        localPath = Application.persistentDataPath + "/PlayerData.json";
        LocalLoadPlayerData();

       bool playerInBase = await fbManager.CheckPlayerInFB();

        if (playerInBase)
        {
            playerDataInFB = playerInBase;
            Debug.Log("FB has PlayerData");
        }
        else
        {
            Debug.Log("FB doesn't have PlayerData");
            playerDataInFB = await fbManager.WriteNewUserAsync(playerData);

            if (playerDataInFB)
                Debug.Log("PlayerData added in FB");       
        }        
        playerProgressManager.SetCurrentProgress(playerData, fbManager, playerDataInFB);
    }            

    public void LocalSavePlayerData()
    {       
        if (playerData != null)
        {
            Debug.Log($"Saving to {localPath}");
            string jsonString = JsonConvert.SerializeObject(playerData);     

            using (FileStream streamFile = new FileStream(localPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(streamFile))
                {
                    writer.Write(jsonString);
                }
            }
            Debug.Log("PlayerData saved successfully!");
        }
        else
        {
            Debug.Log("Add PlayerData");
        }
    }

    public void LocalLoadPlayerData()
    {      
        if (File.Exists(localPath))
        {
            Debug.Log($"loading from file {localPath}");
            string jsonString;
            using (StreamReader reader = new StreamReader(localPath))
            {
                jsonString = reader.ReadToEnd();
            }
            playerData = JsonConvert.DeserializeObject<PlayerData>(jsonString);
            PrintPlayerData(playerData);
        }
        else
        {          
            AddDefaultSetting();
            PrintPlayerData(playerData);
        }       
    }

    private void PrintPlayerData(PlayerData playerData)
    {
        if (playerData == null)
        {
            Debug.LogError("Cannot print - PlayerData is null");
            return;
        }
        int counter = 0;
        Debug.Log($"{++counter}. уровень {playerData.Level}");
        Debug.Log($"{++counter}. текущий прогресс {playerData.Score}");
        Debug.Log($"{++counter}. прогресс для след уровня {playerData.ScoreToNextLevel}");
        Debug.Log($"{++counter}. здоровье {playerData.Health}");
        Debug.Log($"{++counter}. скорость {playerData.Speed}");       
    }

    private void AddDefaultSetting()
    {
        if (playerDefaultSettings == null)
        {
            Debug.LogError("PlayerDefaultSettings is not assigned in inspector!");
            return;
        }
        playerData = new PlayerData(playerDefaultSettings);
        LocalSavePlayerData();
        Debug.Log("Default settings applied successfully");
    }
}
