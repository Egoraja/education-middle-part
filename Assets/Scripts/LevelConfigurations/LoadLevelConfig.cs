using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class LoadLevelConfig : MonoBehaviour
{
    [SerializeField] private string url = "https://docs.google.com/spreadsheets/d/1DixYZwC5jbjCm-xQAE20XLB_yTWLU4xloufKgke0UHU/gviz/tq?tqx=out:csv";
  

    public event System.Action<LevelConfig> OnDataLoaded;
    public event System.Action<string> OnDataLoadFailed;

    private Dictionary<string, int> levelConfigDictionary = new Dictionary<string, int>();

    private bool isDataLoaded = false;
    public bool IsDataLoaded => isDataLoaded;

    private LevelConfig currentLevelConfig;
    public LevelConfig CurrentConfig { get { return currentLevelConfig; } private set { currentLevelConfig = value; } }

    public static LoadLevelConfig Instance { get; private set; }

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

        if (levelConfigDictionary.ContainsKey("enemies") && levelConfigDictionary.ContainsKey("bonuses"))
        {
            currentLevelConfig = new LevelConfig(levelConfigDictionary["enemies"], levelConfigDictionary["bonuses"]);
            isDataLoaded = true;
            Debug.Log($"Врагов: {levelConfigDictionary["enemies"]}, Бонусов: {levelConfigDictionary["bonuses"]}");
            OnDataLoaded?.Invoke(currentLevelConfig);
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
        levelConfigDictionary.Clear();
        string[] lines = csvText.Split('\n');
        char board = '"';
        foreach (string line in lines)
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] parts = line.Split(',');

            if (parts.Length == 2)
            {
                string key = parts[0].Trim(board);
                if (int.TryParse(parts[1].Trim(board), out int value))
                {
                    if (levelConfigDictionary.ContainsKey(key))
                        levelConfigDictionary[key] = value;
                    else
                        levelConfigDictionary.Add(key, value);
                }
                else
                {
                    Debug.LogWarning($"Failed to parse value for key: {key}");
                }
            }
        }
    }
}
