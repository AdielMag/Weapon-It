using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof (Level))]
public class Level_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Level level = (Level)target;

        if (GUILayout.Button("Get Target spawners"))
            level.GetAllSpawners();
    }
}
