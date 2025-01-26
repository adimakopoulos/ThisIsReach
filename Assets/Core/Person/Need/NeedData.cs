using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class NeedData
    {
        private MinMaxCurrWarnTrackerData minMaxCurr;
        private NeedEnum type;

        public NeedData(MinMaxCurrWarnTrackerData minMaxCurr, NeedEnum type)
        {
            this.MinMaxCurr = minMaxCurr;
            this.Type = type;
        }

        public MinMaxCurrWarnTrackerData MinMaxCurr { get => minMaxCurr; set => minMaxCurr = value; }
        public NeedEnum Type { get => type; set => type = value; }
    }
}
