using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    [Serializable]
    public class Dep 
    {

        public string name;
        public MonoBehaviour monoBehaviour;

        public Dep(string name, MonoBehaviour monoBehaviour)
        {
            this.name = name;
            this.monoBehaviour = monoBehaviour;
        }

    }
}
