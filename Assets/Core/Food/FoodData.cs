using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class FoodData 
    {
        private FoodTypes type;
        private int points;

        public FoodData(FoodTypes type, int points)
        {
            this.Type = type;
            this.Points = points;
        }

        public FoodData()
        {

        }

        public FoodTypes Type { get => type; set => type = value; }
        public int Points { get => points; set => points = value; }
    }
}
