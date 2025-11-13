using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProgressManager : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> levelUpActions;
    [SerializeField] private int[] scoreGrades;
    [SerializeField] private int maxLevel;
    [SerializeField] private int firstGrade = 10;
    [SerializeField] private bool useFibonacci = true;
    private InventoryManager inventoryManager;
    private PlayerData playerData;
    private FireBaseManager fireBaseManager;
    private CharacterHealth characterHealth;
    private float scoreToNextLevel = 0;
    private float score;
    private int currentLevel;
    private bool playerDataInFB = false;

    private void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
        characterHealth = GetComponent<CharacterHealth>();
    }
    public void SetCurrentProgress(PlayerData playerData, FireBaseManager fireBaseManager, bool playerDataInFB)
    {
        this.playerData = playerData;
        this.fireBaseManager = fireBaseManager;
        this.playerDataInFB = playerDataInFB;
        this.score = playerData.Score;       
        currentLevel = playerData.Level;        
        SetScoreGrades(currentLevel);
    }

    public void AddNewScore(float score)
    {
        this.score += score;
        if (this.score >= scoreToNextLevel)
        {
            if (currentLevel == maxLevel)
                return;
            UpdateLevel();          
        }        
    }

    public void AddHealth(float healthUpVolume)
    {
        characterHealth.ApplyHealing(healthUpVolume);    
    }

    public void AddShield()
    {
        characterHealth.AddShield();    
    }

    public void AddNewItem(GameObject uIItem)
    {
        bool isReadyTocraft = false;
        inventoryManager.AddNewItem(uIItem, isReadyTocraft);
    }

    private void SetScoreGrades(int currentLevel)
    {
        int a = 0;
        int b = 0;

        scoreGrades = new int[maxLevel];

        for (int i = 0; i < scoreGrades.Length; i++)
        {
            if (useFibonacci == true)
            {
                if (a == 0)
                {
                    a = firstGrade;
                    scoreGrades[i] = a;
                }
                else if (b == 0)
                {
                    b = firstGrade * 2;
                    scoreGrades[i] = b;
                }
                else
                {
                    scoreGrades[i] = a + b;
                    a = b;
                    b = scoreGrades[i];
                }
            }
            else
            {
                a += firstGrade;
                scoreGrades[i] = a;
            }
        }
        UpdateLevel();
    }

    private void UpdateLevel()
    {
        Debug.Log(score + " score");
        if (currentLevel > 0 && currentLevel < scoreGrades.Length)
        {
            for (int i = 0; i < scoreGrades.Length - 1; i++)
            {
                if (score >= scoreGrades[i] && score < scoreGrades[i + 1])
                {
                    currentLevel = i + 2;
                    scoreToNextLevel = scoreGrades[i+1];
                    foreach (var action in levelUpActions )
                    {
                        if (action is ILevelUp levelUp == false) return;
                        levelUp.LevelUp(currentLevel, maxLevel);                    
                    }
                    break;
                }               
            }
        }

        if (scoreToNextLevel == 0)
        {
            scoreToNextLevel = scoreGrades[currentLevel - 1];
            foreach (var action in levelUpActions)
            {
                if (action is ILevelUp levelUp == false) return;
                levelUp.SetCongiguration(currentLevel, maxLevel);
            }
        }       
    }

    public void PrintInfo()
    {
        Debug.Log(score + ">>>>> SCORE");
        Debug.Log(scoreToNextLevel + ">>>>> SCORE TO NEXT LEVEL");
        Debug.Log(currentLevel + ">>>>> LEVEL");   
    }
}
