using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]

public class EnemyViewModel : MonoBehaviour, INotifyPropertyChanged
{
    private string health = "0";
    private string currentLevel = "0";   
    private float fillAmount = 0;

    public event PropertyChangedEventHandler PropertyChanged;
    public static EnemyViewModel InstanceEnemyViewModel { get; private set; }

    private void Awake()
    {
        if (InstanceEnemyViewModel == null)
            InstanceEnemyViewModel = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PrintInfo(SharingPlayerInfo playerInfo)
    { 
        EnemyHealth = playerInfo.Health.ToString();
        FillAmount = playerInfo.Health /playerInfo.MaxHealth;
        CurrentLevel = playerInfo.CurrentLevel.ToString();
    }

    [Binding]
    public string EnemyHealth
    {
        get { return health; }
        set { health = value; OnPropertyChanged("EnemyHealth"); }
    }

    [Binding]
    public string CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; OnPropertyChanged("CurrentLevel"); }
    }   

    [Binding]
    public float FillAmount
    {
        get { return fillAmount; }
        set { fillAmount = value; OnPropertyChanged("FillAmount"); }
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
