using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "CraftSettings", menuName = "Game Data/Create Craft Data")]

public class CraftSettings : ScriptableObject
{
    public List<CraftCombination> CraftCombinations;

    [Serializable]

    public class CraftCombination
    {
        public List<string> Sources;
        public List<GameObject> Results;   
    }
}
