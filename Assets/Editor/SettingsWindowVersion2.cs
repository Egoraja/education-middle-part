using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class SettingsWindowVersion2 : EditorWindow
{
    private string pathToSettings = "Assets/Scripts";
    private readonly string[] searchFolders = new[] { "Assets/Scripts" };
    private ScriptableObject[] foundSettings;
    private string[] settingNames;
    private int selectedIndex = -1;
    private ScriptableObject selectedSetting;
    private Vector2 rightScroll;

    [MenuItem("Window/Game Settings Window version 2")]
    public static void ShowWindow()
    {
        GetWindow<SettingsWindowVersion2>("Game Settings version 2");
    }

    private void OnGUI()
    {
        GUILayout.Label("Game Settings", EditorStyles.boldLabel);
        GUILayout.Space(10f);

        if (GUILayout.Button("Refresh Settings List", GUILayout.Height(25)))
            LoadSettings();

        GUILayout.Space(10f);

        if (foundSettings == null || foundSettings.Length == 0)
        {
            EditorGUILayout.HelpBox("No settings found. Click 'Refresh' to search.", MessageType.Info);
            return;
        }

        DrawDropDownMenu();
        GUILayout.Space(10f);
        DrawSettingsInspector();
    }

    private void DrawDropDownMenu()
    {
        EditorGUILayout.LabelField("Select Settings Asset:", EditorStyles.boldLabel);
        GUILayout.Space(10f);
        int newIndex = EditorGUILayout.Popup(selectedIndex, settingNames);

        if (newIndex != selectedIndex)
        {
            selectedIndex = newIndex;
            selectedSetting = foundSettings[selectedIndex];
        }
    }    

    private void DrawSettingsInspector()
    {
        EditorGUILayout.BeginVertical("box");
        rightScroll = EditorGUILayout.BeginScrollView(rightScroll);

        if (selectedSetting == null)
        {
            EditorGUILayout.HelpBox("Please select a settings from the list", MessageType.None);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            return;
        }

        EditorGUILayout.LabelField(selectedSetting.name, EditorStyles.boldLabel);
        GUILayout.Space(10f);

        var fields = selectedSetting.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

        EditorGUI.BeginChangeCheck();

        foreach (var field in fields)
        {
            if (field.IsPublic == false && field.GetCustomAttribute<SerializeField>() == null)
                continue;

            object value = field.GetValue(selectedSetting);
            Type type = field.FieldType;
            string label = ObjectNames.NicifyVariableName(field.Name);

            EditorGUI.indentLevel++;
            object newValue = DrawField(label, value, type);
            EditorGUI.indentLevel--;

            if (Equals(value, newValue) == false)
            {
                field.SetValue(selectedSetting, newValue);
            }
        }
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(selectedSetting);
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private object DrawField(string label, object value, Type type)
    {
        if (type == typeof(int))
            return EditorGUILayout.IntField(label, (int)value);
        if (type == typeof(float))
            return EditorGUILayout.FloatField(label, (float)value);
        if (type == typeof(string))
            return EditorGUILayout.TextField(label, (string)value);
        if (type == typeof(bool))
            return EditorGUILayout.Toggle(label, (bool)value);
        if (type == typeof(Vector2))
            return EditorGUILayout.Vector2Field(label, (Vector2)value);
        if (type == typeof(Vector3))
            return EditorGUILayout.Vector3Field(label, (Vector3)value);
        if (type.IsEnum)
            return EditorGUILayout.EnumPopup(label, (Enum)value);
        if (typeof(UnityEngine.Object).IsAssignableFrom(type))
            return EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, type, false);

        EditorGUILayout.LabelField(label, $"Unsupported type: {type.Name}");

        return value;
    }

    private void LoadSettings()
    {
        var guids = AssetDatabase.FindAssets("t:ScriptableObject", searchFolders);
        var tempList = new List<ScriptableObject>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.StartsWith(pathToSettings) == false)
                continue;

            var asset = AssetDatabase.LoadAssetAtPath<ScriptableObject>(path);

            if (asset != null && asset.GetType().Name.EndsWith("Settings"))
                tempList.Add(asset);
        }
        foundSettings = tempList.ToArray();
        settingNames = new string[foundSettings.Length];
        for (int i = 0; i < foundSettings.Length; i++)
            settingNames[i] = $"{foundSettings[i].name} ({foundSettings[i].GetType().Name})";

        if (foundSettings.Length > 0)
        {
            selectedIndex = 0;
            selectedSetting = foundSettings[0];
        }
        Debug.Log($"Found {foundSettings.Length} settings files");
        Repaint();
    }

    private void OnFocus()
    {
        LoadSettings();
    }
}
