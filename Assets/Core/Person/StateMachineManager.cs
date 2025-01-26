using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace ThisIsReach
{
    public class StateMachineManager : MonoBehaviour
    {

        private static List<(PersonStateEnum, PersonStateEnum)> InvalidCombinations = new List<(PersonStateEnum, PersonStateEnum)>()
        {
            (PersonStateEnum.SNARED, PersonStateEnum.MOVING)
}       ;
        //private static HashSet<KeyValuePair<PersonStateEnum, PersonStateEnum>> ValidCombinationsHas = new HashSet<KeyValuePair<PersonStateEnum, PersonStateEnum>>()
        [SerializeField]
        public PersonDependenciesManager dependenciesManager;
        public Action<PersonStateEnum> onStateChange;

        private PersonStateEnum currState;

        public PersonStateEnum State
        {
            get => currState;
            private set
            {
                onStateChange.Invoke(value);
                Debug.Log("onStateChange: value");
                value = currState;
            }
        }

        private void OnEnable()
        {
            currState = PersonStateEnum.IDLE;
        }


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void requestValidationAndChangeOfState(PersonStateEnum personStateEnum)
        {
            if (isValidStateTransition(personStateEnum)) {
                Debug.Log("Ivnalid State Transition");
                return;
            }
            currState = personStateEnum;
        }

        private bool isValidStateTransition(PersonStateEnum nextState)
        {
            foreach (var invalidCombination in InvalidCombinations)
            {
                if (currState == invalidCombination.Item1 && nextState == invalidCombination.Item2)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
