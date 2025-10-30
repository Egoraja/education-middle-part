using Firebase.Database;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class FireBaseManager : MonoBehaviour
{
    private string userID;
    private DatabaseReference dbReference;

    private void Awake()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async Task<bool> WriteNewUserAsync(PlayerData playerData)
    {
        try
        {
            string json = JsonConvert.SerializeObject(playerData);
            await dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
            Debug.Log($"FireBase got new user = true");
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log($"Update to FireBase finished whith fail {ex.Message}");
            Debug.Log($"FireBase got new user = false");
            return false;
        }
    }

    public async Task<bool> UpdateUsersDataAsync(PlayerData playerData)
    {
        try
        {
            string json = JsonConvert.SerializeObject(playerData);
            await dbReference.Child("users").Child(userID).SetRawJsonValueAsync(json);
            Debug.Log($"FireBase got new user = true");
            return true;
        }
        catch (Exception ex)
        {
            Debug.Log($"Update to FireBase finished whith fail {ex.Message}");
            Debug.Log($"FireBase got new user = true");
            return false;
        }
    }

    public async Task<bool> UpdateScoreAsync(float newScore)
    {
        try
        {
            if (newScore < 0)
            {
                Debug.Log("Score cannot be negative");
                return false;
            }

            var dbScore = await dbReference.Child("users").Child(userID).Child("Score").GetValueAsync();
            float bestScore = 0f;

            if (dbScore.Exists && dbScore.Value != null)
            {
                bestScore = Convert.ToSingle(dbScore.Value);
            }

            if (newScore > bestScore)
            {
                await dbReference.Child("users").Child(userID).Child("Score").SetValueAsync(newScore);
                Debug.Log($"New record achieved: {newScore}");
                return true;
            }

            Debug.Log($"Current score: {newScore}, Best score: {bestScore}");
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to update score: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> CheckPlayerInFB()
    {
        var dbScore = await dbReference.Child("users").Child(userID).GetValueAsync();

        if (dbScore.Exists)
            return true;
        else
            return false;
    }
}
