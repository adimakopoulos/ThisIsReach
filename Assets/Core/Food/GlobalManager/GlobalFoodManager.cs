using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

namespace ThisIsReach
{
    public class GlobalFoodManager : MonoBehaviour
    {
        public static Dictionary<Vector3, FoodManager> allFoodInScene = new Dictionary<Vector3, FoodManager>();
        public static int numOfSources;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void addFoodInList(FoodManager foodManager)
        {
            if (foodManager == null) {
                Debug.Log("a foodManager was enabled but was found to be null...");
                return;
            }
            allFoodInScene.TryAdd(foodManager.gameObject.transform.position, foodManager);
            numOfSources = allFoodInScene.Count;
        }
        public static void removeFoodInList(FoodManager foodManager)
        {
            if (foodManager == null)
            {
                Debug.Log("a foodManager was disabled but was found to be null...");
                return;
            }
            allFoodInScene.Remove(foodManager.gameObject.transform.position);
            numOfSources = allFoodInScene.Count;
        }

        public static Vector3 getNearestFood(Vector3 startingPos) {
            return Vector3.right;
            //return allFoodInScene.GetValueOrDefault(startingPos);
        } 

    }
}
