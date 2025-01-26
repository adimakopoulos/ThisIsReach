using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class WorkStationData
    {
        bool isInUse;
        WorkStationType type;

        public bool IsInUse { get => isInUse; set => isInUse = value; }
        public WorkStationType Type { get => type; set => type = value; }
    }
}
