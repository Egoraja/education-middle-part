using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseState<Estate> where Estate: Enum
{
    public BaseState(Estate key)
    {
        StateKey = key;
    }
    public Estate StateKey { get; private set; }
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract Estate GetNextState();
    public abstract void OnTriggerEnter(Collider other);
    public abstract void OnTriggerStay(Collider other);
    public abstract void OnTriggerExit(Collider other);
}
