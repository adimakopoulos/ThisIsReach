using UnityEngine;
using UnityEditor;
using TileWorldGeneration;

namespace ThisIsReach
{
    [CustomEditor(typeof(WorldGenerationsManager))]

    public class WorldGenerationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Custom Editor", EditorStyles.boldLabel);

            WorldGenerationsManager worldGenerationManager = (WorldGenerationsManager)target;

            if (GUILayout.Button("Generate world"))
            {
                worldGenerationManager.Generate();
            }
            if (GUILayout.Button("Delete Tiles"))
            {
                worldGenerationManager.DeleteTiles();
            }
        }
    }
}
