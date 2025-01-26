using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TileWorldGeneration

{
    public class WorldGenerationsManager : MonoBehaviour
    {
        [Range(1,50)]
        public int X, Y;
        [Range(1, 100)]
        public int widthOfTile;
        public GameObject TilePrefab;
        public static 
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Generate() { 
            for (int i = 0; i < X; i++)
            {
                for (int j = 0; j < Y; j++)
                {
                    GameObject tileRef = Instantiate<GameObject>(TilePrefab);
                    tileRef.name = "Tile| x:" + i + "y" + j;
                    tileRef.transform.position = new Vector3(i* widthOfTile, 0, j* widthOfTile);
                }
            }
        
        }


        public void DeleteTiles()
        {
            // Find and delete game objects with names starting with "Tile| x:"
            GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject tile in allObjects)
            {
                if (tile.name.StartsWith("Tile|"))
                {
                    DestroyImmediate(tile);
                }
            }
        }

    }
}
