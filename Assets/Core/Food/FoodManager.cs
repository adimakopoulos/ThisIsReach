using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class FoodManager : MonoBehaviour
    {
        FoodData data = new FoodData();
        public FoodTypes type;
        public int amountOfFood;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnEnable()
        {
            data.Type = type;
            data.Points = amountOfFood;
            GlobalFoodManager.addFoodInList(this);
        }
        private void OnDisable()
        {
            GlobalFoodManager.removeFoodInList(this);
        }
    }
}
