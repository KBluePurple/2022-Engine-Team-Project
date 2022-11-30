using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StartSector))]
public class SelectDoorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Start"))
        {
            var startSector = (StartSector)target;
            startSector.StartStage();
        }

        EditorGUILayout.EndHorizontal();
    }
}