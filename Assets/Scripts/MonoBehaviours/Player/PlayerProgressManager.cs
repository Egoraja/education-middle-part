using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Profiling.HierarchyFrameDataView;

public class PlayerProgressManager : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private int maxLevel;
    [SerializeField] private int firstGrade = 10;
    [SerializeField] private bool useFibonacci = true;
    [SerializeField] private int[] scoreGrades;
    [SerializeField] private List<MonoBehaviour> levelUpActions;
    private PlayerData playerData;
    private FireBaseManager fireBaseManager;
    private CharacterHealth characterHealth;
    private ViewModel viewModel;
    private float currentHealth;
    private float maxHealth;
    private float scoreToNextLevel = 0;
    private float currentScore;
    private int currentLevel;
    private bool playerDataInFB = false;
    private bool isAlive = true;
    private bool isMoving = true;

    public bool IsAlive
    { get { return isAlive; } set { isAlive = value; } }

    public bool IsMoving
    { get { return isMoving; } set { isMoving = value; playerController.IsMoving = value; } }


    private void Start()
    {       
        characterHealth = GetComponent<CharacterHealth>();
        viewModel = ViewModel.InstanceViewModel;
        if (viewModel == null)        
            Debug.LogError($"View Model is null");      
    }

    public void SetCurrentProgress(PlayerData playerData, FireBaseManager fireBaseManager, bool playerDataInFB)
    {
        this.playerData = playerData;
        this.fireBaseManager = fireBaseManager;
        this.playerDataInFB = playerDataInFB;
        this.currentScore = playerData.Score;       
        currentLevel = playerData.Level;       
        SetScoreGrades(currentLevel);        
    }

    public void AddNewScore(float score)
    {
        this.currentScore += score;
        if (this.currentScore >= scoreToNextLevel)
        {
            if (currentLevel == maxLevel)
                return;
            UpdateLevel();          
        }        
    }

    public void AddHealth(float healthUpValue)
    {
        characterHealth.ApplyHealing(healthUpValue);
        PrintInfo();
    }

    public void ApplyDamage(float damageValue)
    {
        characterHealth.ApplyDamage(damageValue);
        PrintInfo();
    }

    public void PlayerIsDead()
    {
        playerController.PlayerIsDead();
        playerController.IsMoving = false;
        isAlive = false;
    }

    public void AddShield()
    {
        characterHealth.AddShield();    
    }

    public void AddNewItem(GameObject uIItem)
    {
        bool isReadyTocraft = false;
        inventoryManager.AddNewItem(uIItem, isReadyTocraft, gameObject);
    }

    public void PrintInfo()
    {
        currentHealth = characterHealth.Health;
        maxHealth = characterHealth.MaxHealth;
        viewModel.Health = currentHealth.ToString();
        viewModel.FillAmount = currentHealth / maxHealth;
        viewModel.CurrentLevel = currentLevel.ToString();
        viewModel.CurrentScore = currentScore.ToString();
        if (scoreToNextLevel >= scoreGrades[scoreGrades.Length-1])
            viewModel.ScoreToNextLevel = "Max level";
        else
            viewModel.ScoreToNextLevel = scoreToNextLevel.ToString();
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

        foreach (var action in levelUpActions)
        {
            if (action is ILevelUp levelUp == false) return;
            levelUp.SetConfiguration(currentLevel, maxLevel);
            Debug.Log("Action action");
        }
        UpdateLevel();
    }

    private void UpdateLevel()
    {        
        if (currentLevel > 0 && currentLevel < scoreGrades.Length)
        {
            for (int i = 0; i < scoreGrades.Length - 1; i++)
            {
                if (currentScore >= scoreGrades[i] && currentScore < scoreGrades[i + 1])
                {
                    currentLevel = i + 2;
                    scoreToNextLevel = scoreGrades[i+1];
                    Debug.Log("Score to next level = " + scoreToNextLevel);
                    foreach (var action in levelUpActions )
                    {
                        if (action is ILevelUp levelUp == false) return;
                        levelUp.LevelUp(currentLevel, maxLevel);                    
                    }
                    break;
                }
                if (currentScore == 0 && currentLevel == 1)
                {
                    scoreToNextLevel = scoreGrades[0];
                }
            }
        }
        PrintInfo();
    }
}
