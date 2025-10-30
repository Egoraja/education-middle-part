using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class LoadNewData : MonoBehaviour
{
    private int count = 0;
        
    private async void PrintText()
    {       
        await Task.Run(PrintFinishText);    
    }

    private void PrintFinishText()
    {
        Thread.Sleep(4000);
        Debug.Log(count + " count");        
        Debug.Log("Finish");    
    }

    private async void LoadData()
    {
        Debug.Log("Start loading");
        await Task.Delay(2000);
        if (count < 5)
        {
            count++;
            Debug.Log("Loaded");
            LoadData();
        }
        else
        {
           PrintText();
        }
    }
        
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
         LoadData();
        }
    }
}
