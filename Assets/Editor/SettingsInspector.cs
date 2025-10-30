using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(EnemySettings))]
public class SettingsInspector : Editor
{
    private SerializedProperty health;

    private void OnEnable()
    {
        health = serializedObject.FindProperty("EnemyHealth");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();        
        GUILayout.Space(8f); 
        GUILayout.BeginHorizontal("box");
        GUILayout.Space(40f);
        if (GUILayout.Button("Set health to 100"))
        {
            health.floatValue = 100f;
        }
        GUILayout.Space(40f);
        GUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();
    }
}
