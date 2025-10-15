using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using System.Threading.Tasks;

public class SaveAndLoadData : MonoBehaviour
{
    [SerializeField] private string url = "https://docs.google.ru/spreadsheets/d/1DixYZwC5jbjCm-xQAE20XLB_yTWLU4xloufKgke0UHU/gviz/tq?tqx=out:csv";

    public event System.Action<PlayerDataConfig> OnDataLoaded;
    public event System.Action<string> OnDataLoadFailed;

    private Dictionary<string, float> characterData = new Dictionary<string, float>();

    private bool isDataLoaded = false;
    public bool IsDataLoaded => isDataLoaded;

    private PlayerDataConfig currentConfig;
    public PlayerDataConfig CurrentConfig { get { return currentConfig; } private set { currentConfig = value; } }

    public static SaveAndLoadData Instance { get; private set; }

    private void Awake()
    {       
        if (Instance == null)        
            Instance = this;                  
        else        
            Destroy(gameObject);
        return;        
    }

    private async void Start()
    {
        await LoadDataAsync();
    }

    private async Task<bool> LoadDataAsync()
    { 
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            var operation = www.SendWebRequest();

            while (operation.isDone == false)
            {               
                await Task.Yield();
                Debug.Log("Loading...");
            }

            Debug.Log("Request completed, starting delay");
            await Task.Delay(2000);

            Debug.Log("Delay finished");

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Ошибка: " + www.error);
                OnDataLoadFailed?.Invoke(www.error);
                return false;
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
            currentConfig = new PlayerDataConfig(characterData["speed"], characterData["health"]);
            isDataLoaded = true;
            Debug.Log($"Скорость: {characterData["speed"]}, Здоровье: {characterData["health"]}");
            OnDataLoaded?.Invoke(currentConfig);
            return true;
        }
        else
        {
            Debug.Log("Не удалось загрузить параметры");
            OnDataLoadFailed?.Invoke("Missing required parameters: speed or health");
            return false;
        }
    }
    

    private void ParseCSV(string csvText)
    {
        characterData.Clear();
        string[] lines = csvText.Split('\n');
        char board = '"';
        foreach (string line in lines)
        {          
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');

            if (parts.Length == 2)
            {               
                string key = parts[0].Trim(board);               
                if (float.TryParse(parts[1].Trim(board), out float value))
                {
                    if (characterData.ContainsKey(key))
                        characterData[key] = value;                                      
                    else                    
                        characterData.Add(key, value);                    
                }
                else
                {
                    Debug.LogWarning($"Failed to parse value for key: {key}");
                }
            }           
        }        
    }
}
