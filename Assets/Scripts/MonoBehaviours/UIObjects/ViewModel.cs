using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class ViewModel : MonoBehaviour, INotifyPropertyChanged
{
    private string health = "1";
    private string currentLevel = "1";
    private string currentScore = "0";
    private string scoreToNextLevel = "0";
    private float fillAmount = 0;    
    public event PropertyChangedEventHandler PropertyChanged;

    public static ViewModel InstanceViewModel { get; private set; }

    private void Awake()
    {
        if (InstanceViewModel == null)
            InstanceViewModel = this;
        else
        {
            Destroy(gameObject);
            return; 
        }
    }

    [Binding]
    public string Health
    {
        get { return health; }
        set { health = value; OnPropertyChanged("Health");  }
    }

    [Binding]
    public string CurrentLevel
    {
        get { return currentLevel; }
        set { currentLevel = value; OnPropertyChanged("CurrentLevel"); }
    }

    [Binding]
    public string CurrentScore
    {
        get { return currentScore; }
        set { currentScore = value; OnPropertyChanged("CurrentScore"); }
    }

    [Binding]
    public string ScoreToNextLevel
    {
        get { return scoreToNextLevel; }
        set { scoreToNextLevel = value; OnPropertyChanged("ScoreToNextLevel"); }
    }

    [Binding]
    public float FillAmount
    {
        get { return fillAmount; }
        set { fillAmount = value; OnPropertyChanged("FillAmount");  }
    }

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }    
    }
}
