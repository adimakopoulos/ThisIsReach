using UnityEngine;
using UnityEditor;
using SimpleMovementNS;

namespace SimpleMovementNS
{
    [CustomEditor(typeof(GlobalPathingService))]
    public class GlobalPathingServiceEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector for GlobalPathingService fields
            DrawDefaultInspector();

            EditorGUILayout.Space();

            // Get a reference to the GlobalPathingService component being inspected
            GlobalPathingService pathingService = (GlobalPathingService)target;

            // Create a button labeled "Create Grid"
            if (GUILayout.Button("Create Grid"))
            {
                // Call the CreateGrid method when the button is pressed
                pathingService.CreateGrid();
            } 
            if (GUILayout.Button("Clear Grid"))
            {
                // Call the CreateGrid method when the button is pressed
                pathingService.DeleteGrid();
            }
        }
    }
}
