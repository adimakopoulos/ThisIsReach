using UnityEngine;
using UnityEditor;
using SimpleWorldGenerationNS;

namespace SimpleWorldGenerationNS
{
    [CustomEditor(typeof(WorldGenerationsManager))]
    public class WorldGenerationEditor : Editor
    {
        // Temporary fields to hold our material selections in the editor.
        private Material material1;
        private Material material2;
        private Material material3;
        private int selectedMaterialIndex = 0; // 0 = material1, 1 = material2, 2 = material3

        private void OnEnable()
        {
            // Adjust the paths if needed to match your project structure.
            material1 = AssetDatabase.LoadAssetAtPath<Material>("Packages/com.alex.world.generation/Prefabs/Tile/GrassMat1.mat");
            material2 = AssetDatabase.LoadAssetAtPath<Material>("Packages/com.alex.world.generation/Prefabs/Tile/GrassMat2.mat");
            material3 = AssetDatabase.LoadAssetAtPath<Material>("Packages/com.alex.world.generation/Prefabs/Tile/GrassMat3.mat");

        }

        public override void OnInspectorGUI()
        {
            // Draw the default inspector (if you have other serialized properties)
            DrawDefaultInspector();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Custom Editor", EditorStyles.boldLabel);

            WorldGenerationsManager worldGenerationManager = (WorldGenerationsManager)target;

            // Buttons for existing functionality
            if (GUILayout.Button("Generate world"))
            {
                worldGenerationManager.Generate();
            }
            if (GUILayout.Button("Delete Tiles"))
            {
                worldGenerationManager.DeleteTiles();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tile Painting", EditorStyles.boldLabel);

            // Allow user to assign 3 materials
            material1 = (Material)EditorGUILayout.ObjectField("Material 1", material1, typeof(Material), false);
            material2 = (Material)EditorGUILayout.ObjectField("Material 2", material2, typeof(Material), false);
            material3 = (Material)EditorGUILayout.ObjectField("Material 3", material3, typeof(Material), false);

            // Provide a popup for choosing one of the materials
            string[] options = new string[] { "Material 1", "Material 2", "Material 3" };
            selectedMaterialIndex = EditorGUILayout.Popup("Selected Material", selectedMaterialIndex, options);

            // When the user clicks "Paint Tiles", pass the chosen material to the manager.
            if (GUILayout.Button("Paint Tiles"))
            {
                Material selectedMaterial = null;
                switch (selectedMaterialIndex)
                {
                    case 0:
                        selectedMaterial = material1;
                        break;
                    case 1:
                        selectedMaterial = material2;
                        break;
                    case 2:
                        selectedMaterial = material3;
                        break;
                }

                if (selectedMaterial != null)
                {
                    // Call the method to paint the tiles (you must implement this method in WorldGenerationsManager)
                    worldGenerationManager.PaintTiles(selectedMaterial);
                }
                else
                {
                    Debug.LogWarning("Please assign a material to the selected slot before painting.");
                }
            }
        }
    }
}
