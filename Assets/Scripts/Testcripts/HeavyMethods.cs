using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System;

public class HeavyMethods : MonoBehaviour
{
    private void PrintText()
    {
        for (int i = 0; i < 5; i++)
        {
            Thread.Sleep(1500);
            Debug.Log($"heavy async void {i + 1}/5");
        }
    }

    private async void HeavyMethods1()
    {
        await Task.Run(PrintText);        
    }

    private void HeavyUniRXMethods()
    {
        IObservable<string> heavyRXMethod1 = Observable.Start(() =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(4));
            Debug.Log("RX 1 finished");
            return "HeavyRX method 1";
        });

        IObservable<string> heavyRXMethod2 = Observable.Start(() =>
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Debug.Log("RX 2 finished");
            return "HeavyRX method 2";
        });

        Observable.WhenAll(heavyRXMethod1, heavyRXMethod2)
            .ObserveOnMainThread()
            .Subscribe(rx =>
            {
                Debug.Log(rx[0]);
                Debug.Log(rx[1]);
            });
    }           
 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            HeavyMethods1();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            HeavyUniRXMethods();        
        }
    }

    
}
