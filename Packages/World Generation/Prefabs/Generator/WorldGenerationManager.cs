using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleWorldGenerationNS
{
    public class WorldGenerationsManager : MonoBehaviour
    {
        [Range(1, 50)]
        public int X=20, Y=20;
        [Range(1, 100)]
        public int widthOfTile=1;
        public GameObject TilePrefab;

        // Start is called before the first frame update
        void Start()//
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Generate()
        {
            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    GameObject tileRef = Instantiate(TilePrefab);
                    tileRef.name = "Tile| x:" + i + " y:" + j;
                    tileRef.transform.position = new Vector3(i * widthOfTile, 0, j * widthOfTile);
                    tileRef.transform.parent = this.gameObject.transform;
                }
            }
        }

        public void DeleteTiles()
        {
            // Find and delete game objects with names starting with "Tile|"
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject tile in allObjects)
            {
                if (tile.name.StartsWith("Tile|"))
                {
                    DestroyImmediate(tile);
                }
            }
        }

        /// <summary>
        /// Applies the given material to all tiles in the scene.
        /// </summary>
        /// <param name="material">The material to use for painting the tiles.</param>
        public void PaintTiles(Material material)
        {
            // Find all game objects in the scene
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject tile in allObjects)
            {
                if (tile.name.StartsWith("Tile|"))
                {
                    Renderer renderer = tile.GetComponentInChildren<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = material;
                    }
                    else
                    {
                        Debug.LogWarning("Tile " + tile.name + " does not have a Renderer component.");
                    }
                }
            }
        }

        /// <summary>
        /// Resets the position of all generated tiles based on the grid coordinates
        /// stored in their names and the current widthOfTile.
        /// </summary>
        public void ResetTilePositions()
        {
            Debug.LogWarning("Resetting tiles position...");
            // Find all game objects in the scene.
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
            foreach (GameObject tile in allObjects)
            {
                if (tile.name.StartsWith("Tile|"))
                {
                    // Expected name format: "Tile| x:0 y:0"
                    // Remove the "Tile| " part then split by " x:" and " y:".
                    string nameWithoutPrefix = tile.name.Substring("Tile| ".Length); // remove "Tile| " (including the space)
                    // Now expected string is "x:0 y:0"
                    // Remove the "x:" part and split at " y:"
                    if (nameWithoutPrefix.StartsWith("x:"))
                    {
                        string withoutX = nameWithoutPrefix.Substring(2); // remove "x:"
                        string[] parts = withoutX.Split(new string[] { " y:" }, System.StringSplitOptions.None);

                        if (parts.Length == 2)
                        {
                            if (int.TryParse(parts[0].Trim(), out int gridX) &&
                                int.TryParse(parts[1].Trim(), out int gridY))
                            {
                                // Calculate the position based on the grid coordinate and widthOfTile.
                                Vector3 newPosition = new Vector3(gridX * widthOfTile, 0, gridY * widthOfTile);
                                tile.transform.position = newPosition;
                            }
                            else
                            {
                                Debug.LogWarning("Failed to parse grid coordinates for tile: " + tile.name);
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Tile name format incorrect for: " + tile.name);
                        }
                    }
                }
            }
        }
    }
}
