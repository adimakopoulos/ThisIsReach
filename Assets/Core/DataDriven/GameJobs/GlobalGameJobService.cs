using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class GlobalGameJobService : MonoBehaviour
    {
        List<GameJobData> gamejobs = new List<GameJobData>();
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public void createJob(GameJobData gameJobData) {
            gamejobs.Add(gameJobData);
        }
        public void cancelJobe(GameJobData gameJobData) {
            gamejobs.Remove(gameJobData);
        }
        public void tickJob(GameJobData gameJobData,int workpoints) {
            // Find the index of the specified job in the list

        }

    }
}
