using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ThisIsReach
{
    public class WorkStationManager : MonoBehaviour
    {
        WorkStationData data;
        public WorkStationType type;
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
            data = new WorkStationData();
            data.IsInUse = false;
            data.Type = type;
            GlobalWorkStationManager.addWorkStationInList(this);
        }
        private void OnDisable()
        {
            GlobalWorkStationManager.removeWorkStationInList(this);
        }

    }
}
