using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class WorkerPoolManager : MonoBehaviour
    {
        [SerializeField] private GameObject workerPrefab; // Assign your prefab in the Inspector
        [SerializeField] private int poolSize = 10;
        private List<GameObject> workerPool;

        // Make the Pool accessible to other scripts in the scene
        public static WorkerPoolManager Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Dupicate instance was found!");
                Destroy(gameObject); // Prevent duplicates if there are any
            }
        }

        void Start()
        {
            workerPool = new List<GameObject>();
            PopulatePool();
        }

        private void PopulatePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject worker = Instantiate(workerPrefab);
                worker.SetActive(false);  // Disable the object initially
                worker.transform.SetParent(transform); // Keep hierarchy clean
                workerPool.Add(worker);
            }
        }

        //Get object from pool
        public GameObject GetWorker()
        {
            for (int i = 0; i < workerPool.Count; i++)
            {
                if (!workerPool[i].activeInHierarchy)
                {
                    return workerPool[i];
                }
            }
            //If no available object found, create one, then return it
            GameObject worker = Instantiate(workerPrefab);
            worker.transform.SetParent(transform);
            workerPool.Add(worker);
            return worker;
        }

        public void ReturnWorkerToPool(GameObject worker)
        {
            worker.SetActive(false);
            worker.transform.SetParent(transform);
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
