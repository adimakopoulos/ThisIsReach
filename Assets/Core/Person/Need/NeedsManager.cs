using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

namespace ThisIsReach
{
    public class NeedsManager : MonoBehaviour
    {
        public int tickRate = 1;
        [SerializeField]
        public PersonDependenciesManager dependenciesManager;
        private Dictionary<NeedEnum, NeedData> needs = new Dictionary<NeedEnum, NeedData>();
        public Dictionary<NeedEnum, NeedData> UnSitifiedNeeds = new Dictionary<NeedEnum, NeedData>();

        public Action<NeedEnum> warn;
        public Action<NeedEnum> minimumValueReachedForNeed;
        public Action<NeedEnum> comfortableValueReachedForNeed;
        
        private void OnEnable()
        {
            InvokeRepeating("tick", 0, 1);
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void EnableNeed(NeedData needData)
        {
            if (needs.ContainsKey(needData.Type))
            {
                Debug.Log("Need Already Enabled: " + needData.Type);
            }
            else
            {
                Debug.Log("Need Eneblaed: " + needData.Type);
                needs.Add(needData.Type, needData);
            }
        }

        public void DisableNeed(NeedEnum needsEnum)
        {
            if (needs.ContainsKey(needsEnum))
            {
                needs.Remove(needsEnum);
                Debug.Log("Need Remove: " + needsEnum);
            }
            else
            {
                Debug.Log("Need Not Present, so it cant be removed : " + needsEnum);
            }
        }

        public void ReplanishNeed(NeedEnum needsEnum, int amount)
        {
            if (needs != null && needs.ContainsKey(needsEnum))
            {
                Debug.Log("Replanishing need: " + needsEnum + " amount:"+ amount);
                needs[needsEnum].MinMaxCurr.Curr = amount;
                if (needs[needsEnum].MinMaxCurr.Curr > needs[needsEnum].MinMaxCurr.Warn) {
                    comfortableValueReachedForNeed?.Invoke(needsEnum);
                    UnSitifiedNeeds.Remove(needsEnum);
                }
            }
            else
            {
                Debug.LogWarning("Cannot replanish Need, because it Does not exist, need:  " + needsEnum);
            }
        }

        private void tick()
        {
            //Guard
            if (needs == null || needs.Count == 0)
            {
                Debug.LogWarning("tick was invoked but NeedsState is empty");
                return;
            }

            foreach (var item in needs)
            {
                item.Value.MinMaxCurr.Curr -= tickRate;
                if (item.Value.MinMaxCurr.Curr <= item.Value.MinMaxCurr.Warn)
                {
                    if (!UnSitifiedNeeds.ContainsKey(item.Key)) {
                        UnSitifiedNeeds.TryAdd(item.Key, item.Value);
                        warn?.Invoke(item.Key);
                    }
                }
                if (item.Value.MinMaxCurr.Curr <= item.Value.MinMaxCurr.Min)
                {
                    minimumValueReachedForNeed?.Invoke(item.Key);
                }
            }
        }

        public Dictionary<NeedEnum, NeedData> GetNeedsUI
        {
            get { return needs; }
        }

    }


}
