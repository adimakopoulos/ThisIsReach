using SimpleMovementNS;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ThisIsReach
{
    public class FarmManager : MonoBehaviour , IBuilding
    {
        private bool isSelected = false;
        private List<GameObject> workers = new List<GameObject>();
        private float secondtoReposition = 3f;
        [SerializeField]private float secondtoRepositionPeriod = 3f;
        [SerializeField]private FarmData farmData;

        public int Name { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void BuildStructure()
        {
            throw new System.NotImplementedException();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            #region UserInputs
            if (isSelected && Input.GetKeyDown(KeyCode.B) && farmData.maxWorkers > workers.Count)
            {
                GameObject worker = WorkerPoolManager.Instance.GetWorker();
                workers.Add(worker);
                worker.transform.position = transform.position;
                worker.SetActive(true);
                farmData.currWorker = workers.Count;
                Debug.Log("Buying 1 worker for selected farm");
            }

            if (isSelected && Input.GetKeyDown(KeyCode.X))
            {
                DeleteFarm();
            }
            #endregion

            MoveWorkersRandomlyInFarm();
            productionCycle();

        }

        private void productionCycle()
        {
            if (farmData.currWorker > 0) { 
                if (farmData.currProductionCycle < 0)
                {
                    farmData.currProductionCycle = FarmData.productionCycleBaseTime;
                    int wheatAmount = farmData.productionAmountBase * farmData.currWorker;
                    PlayerManager.instance.AddResource(ResourceType.WHEAT, wheatAmount);
                    UIPoolManager.Instance.GetUIText().GetComponent<FloatingTextManager>().ShowText("+ " + wheatAmount + "Wheat", this.transform);
                }
                else {
                    farmData.currProductionCycle -= Time.deltaTime;
                }
            }
        }

        public void DeleteFarm()
        {
            foreach (var collectionWorker in workers)
            {
                WorkerPoolManager.Instance.ReturnWorkerToPool(collectionWorker);
            }
            workers = new List<GameObject>();
            GlobalPathingService.instance.UnBlockAllNavCellsOnTile(gameObject.transform.position);
            
            UIPoolManager.Instance.GetUIText().GetComponent<FloatingTextManager>().ShowText("Farm destroyed", this.transform);
            Debug.Log("deleting farm");
            gameObject.SetActive(false);
        }

        private void MoveWorkersRandomlyInFarm()
        {
            if (secondtoReposition < 0 && workers.Count > 0)
            {
                foreach (GameObject worker in workers)
                {
                    worker.GetComponent<MovementManager>().ResolvePathIgnoringNavCells(new Vector3(transform.position.x + Random.Range(0f, 1f), 0, transform.position.z + Random.Range(0f, 1f)));
                }
                secondtoReposition = secondtoRepositionPeriod;
            }
            else
            {
                secondtoReposition -= Time.deltaTime;
            }
        }

        private void OnEnable()
        {
            farmData = new FarmData();
            workers = new List<GameObject>();
        }
        public void HireWorkier() { 
        
        }

        public void SelecteBuilding()
        {
            isSelected = true;
        }

        public void DeselecteBuilding()
        {
            isSelected = false;
        }

        public void CreateWorker()
        {
            
        }
    }
}
