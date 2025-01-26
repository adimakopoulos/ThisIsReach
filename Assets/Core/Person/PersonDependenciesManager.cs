using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SimpleMovementNS;

namespace ThisIsReach
{
    public class PersonDependenciesManager : MonoBehaviour
    {

        [SerializeField]
        public StateMachineManager stateMachineManager;
        public WorkManager workManager;
        public NeedsManager needsManager;
        public MovementManager movementManager;
        public AIManager aIManager;
    }
}

