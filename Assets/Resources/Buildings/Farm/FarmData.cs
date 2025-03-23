using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    [Serializable]
    public class FarmData 
    {
        public int health=100;
        public int currhealth = 100;
        public int currStorage = 0;
        public int maxStorage = 10;
        public int maxWorkers = 5;
        public int currWorker =0;
        public const float productionCycleBaseTime = 10f;
        public float currProductionCycle = 10f;
        public int productionAmountBase = 1;



    }

    public enum ResourceType { 
        STONE,
        WHEAT,
        WOOD,
    }
}
