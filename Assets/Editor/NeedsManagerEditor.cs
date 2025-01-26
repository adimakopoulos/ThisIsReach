using UnityEngine;
using UnityEditor;
using ThisIsReach;

[CustomEditor(typeof(NeedsManager))]
public class NeedsManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); // Call the base implementation to draw the default inspector GUI

        NeedsManager needsManager = (NeedsManager)target;

        GUILayout.Label("NeedsState:");

        foreach (var kvp in needsManager.GetNeedsUI)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(kvp.Key.ToString(), GUILayout.Width(100));
            EditorGUILayout.LabelField("Min:", GUILayout.Width(30));
            kvp.Value.MinMaxCurr.Min = EditorGUILayout.IntField(kvp.Value.MinMaxCurr.Min, GUILayout.Width(50));
            EditorGUILayout.LabelField("Max:", GUILayout.Width(30));
            kvp.Value.MinMaxCurr.Max = EditorGUILayout.IntField(kvp.Value.MinMaxCurr.Max, GUILayout.Width(50));
            EditorGUILayout.LabelField("Curr:", GUILayout.Width(30));
            kvp.Value.MinMaxCurr.Curr = EditorGUILayout.IntField(kvp.Value.MinMaxCurr.Curr, GUILayout.Width(50));
            EditorGUILayout.LabelField("Warn:", GUILayout.Width(30));
            kvp.Value.MinMaxCurr.Warn = EditorGUILayout.IntField(kvp.Value.MinMaxCurr.Warn, GUILayout.Width(50));
            EditorGUILayout.LabelField("Comfortable:", GUILayout.Width(30));
            kvp.Value.MinMaxCurr.Comfortable = EditorGUILayout.IntField(kvp.Value.MinMaxCurr.Comfortable, GUILayout.Width(50));
            EditorGUILayout.EndHorizontal();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(needsManager);
        }
    }
}
