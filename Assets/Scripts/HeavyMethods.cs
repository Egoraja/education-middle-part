using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class HeavyMethods : MonoBehaviour
{
    private void PrintText()
    {
        for (int i = 0; i < 5; i++)
        {
            Thread.Sleep(1500);
            Debug.Log($"I'm here {i + 1}");
        }
    }

    private async void HeavyMethods1()
    {
        await Task.Run(PrintText);        
    }
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            HeavyMethods1();
        }        
    }
}
