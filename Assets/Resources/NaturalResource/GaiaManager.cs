using SimpleMovementNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class GaiaManager : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject[] ResourcesPrefabs;

        public static int MaxStones=3;
        public static int MaxTrees=15;
        // Start is called before the first frame update
        void Start()
        {
            Invoke("generateResourcesOnMapWithRandomPositions",4f);
        }


        private void generateResourcesOnMapWithRandomPositions()
        {
            for (int i = 0; i < MaxStones; i++)
            {
                Vector3 randomPos = new Vector3(Random.Range(0, 20), 0, Random.Range(0, 20));
                if (GlobalPathingService.instance.CanBuildOnTile(randomPos))
                {

                    GlobalPathingService.instance.BlockAllNavCellsOnTile(randomPos); 
                    Instantiate(ResourcesPrefabs[0], randomPos, Quaternion.identity);
                }
            }
            for (int i = 0; i < MaxTrees; i++)
            {
                Vector3 randomPos = new Vector3(Random.Range(0f, 20f), 0f, Random.Range(0f, 20f));
                if (GlobalPathingService.instance.CanBuildOnNavCell(randomPos))
                {
                    GlobalPathingService.GetClosestNavCellPos(randomPos);
                    GlobalPathingService.instance.BlockNavCell(randomPos);
                    Instantiate(ResourcesPrefabs[1], randomPos, Quaternion.identity);
                }

            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
