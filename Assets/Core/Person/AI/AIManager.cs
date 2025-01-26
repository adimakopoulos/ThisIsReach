using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ThisIsReach
{
    public class AIManager : MonoBehaviour
    {
        [SerializeField]
        public PersonDependenciesManager dependenciesManager;
        private AIEnum personState = AIEnum.Idle;
        public Action <Vector3>onNewDestinationSet;
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
            InvokeRepeating("think", 3, 3);
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        public void think() {
            checkNeeds();
        }  

        public void checkNeeds() {
            if (personState == AIEnum.SatisfyingNeeds)
            {
                return;
            }
            foreach (var kvp in dependenciesManager.needsManager.UnSitifiedNeeds)
            {
                // Check if the current need's current value is below the warning value
                if (kvp.Value.MinMaxCurr.Curr < kvp.Value.MinMaxCurr.Warn)
                {
                    Debug.Log("Found need below warning value: " + kvp.Key);
                    personState = AIEnum.SatisfyingNeeds;
                    Vector3 foodPos = GlobalFoodManager.getNearestFood(this.transform.position);// GlobalFoodManager.allFoodInScene.First().Value.gameObject.GetComponent<Transform>().position;
                    onNewDestinationSet?.Invoke(foodPos);
                    return; // Exit the loop after finding the first need below warning value
                }
            }
        }


    }
}
