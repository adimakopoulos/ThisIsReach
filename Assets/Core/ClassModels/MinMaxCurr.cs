using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class MinMaxCurrWarnTrackerData
    {
        private int min, max, curr, warn, comfortable;
        public MinMaxCurrWarnTrackerData() { }

        public MinMaxCurrWarnTrackerData(int min, int max, int curr, int warn, int comfortable)
        {
            this.min = min;
            this.max = max;
            this.curr = curr;
            this.warn = warn;
            this.comfortable = comfortable;
        }

        public int Min
        {
            get => min;
            set
            {
                if (!(value > max)) { min = value; } else { min = max; }
            }
        }
        public int Max
        {
            get => max;
            set
            {
                if (!(value < min)) { max = value; } else { max = min; }
            }
        }
        public int Curr
        {
            get => curr;
            set
            {
                if (!(value > max) && !(value < min)) { curr = value; }
                else if (value >= max) { curr = max; }
                else if (value <= min) { curr = min; }
            }
        }
        public int Warn
        {
            get => warn;
            set
            {
                if (!(value > max) && !(value < min)) { warn = value; }
                else if (value >= max) { warn = max; }
                else if (value <= min) { warn = min; }
            }
        }
        public int Comfortable
        {
            get => comfortable;
            set
            {
                if (!(value > max) && !(value < min)) { comfortable = value; }
                else if (value >= max) { comfortable = max; }
                else if (value <= min) { comfortable = min; }
            }
        }




    }
}
