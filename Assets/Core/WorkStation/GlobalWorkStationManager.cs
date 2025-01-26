using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class GlobalWorkStationManager : MonoBehaviour
    {
        public static Dictionary<int, WorkStationManager> allWorkStations = new Dictionary<int, WorkStationManager>();
        public static int numOfSources;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static void addWorkStationInList(WorkStationManager workStationManager)
        {
            allWorkStations.TryAdd(workStationManager.GetInstanceID(), workStationManager);
            numOfSources = allWorkStations.Count;
        }
        public static void removeWorkStationInList(WorkStationManager workStationManager)
        {
            allWorkStations.Remove(workStationManager.GetInstanceID());
            numOfSources = allWorkStations.Count;
        }
    }
}
