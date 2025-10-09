using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using System.Threading.Tasks;

public class SaveAndLoadData : MonoBehaviour
{
    [SerializeField] private string url = "https://docs.google.com/spreadsheets/d/1DixYZwC5jbjCm-xQAE20XLB_yTWLU4xloufKgke0UHU/gviz/tq?tqx=out:csv";

    private Dictionary<string, float> characterData = new Dictionary<string, float>();

    private async void Start()
    {        
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            var operation = www.SendWebRequest();

            while (!operation.isDone)
            {
                await Task.Yield();                
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка загрузки: " + www.error);
                return;
            }
            else
            {
                string csvText = www.downloadHandler.text;
                Debug.Log("CSV данные:\n" + csvText);
                ParseCSV(csvText);
            }
        }

        if (characterData.ContainsKey("speed") && characterData.ContainsKey("health"))
        {
            Debug.Log($"Скорость: {characterData["speed"]}, Здоровье: {characterData["health"]}");
        }
        else
        {
            Debug.Log("Не удалось загрузить все параметры персонажа");
        }
    }
    

    private void ParseCSV(string csvText)
    {
        string[] lines = csvText.Split('\n');
        char board = '"';
        foreach (string line in lines)
        {          
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');

            if (parts.Length == 2)
            {               
                string key = parts[0].Trim(board);
                Debug.Log(key);
                if (float.TryParse(parts[1].Trim(board), out float value))
                {
                    characterData.Add(key, value);                                
                }
            }           
        }
        foreach (var item in characterData)
        {
            Debug.Log(item.Key + item.Value);
        }
    }
}
