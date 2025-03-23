using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class UIPoolManager : MonoBehaviour
    {



        [SerializeField] private GameObject UIPrefab; // Assign your prefab in the Inspector
        [SerializeField] private int poolSize = 10;
        private List<GameObject> uiTextPool;

        // Make the Pool accessible to other scripts in the scene
        public static UIPoolManager Instance;

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
            uiTextPool = new List<GameObject>();
            PopulatePool();
        }

        private void PopulatePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject uiText = Instantiate(UIPrefab);
                uiText.SetActive(false);  // Disable the object initially
                uiText.transform.SetParent(transform); // Keep hierarchy clean
                uiTextPool.Add(uiText);
            }
        }

        //Get object from pool
        public GameObject GetUIText()
        {
            for (int i = 0; i < uiTextPool.Count; i++)
            {
                if (!uiTextPool[i].activeInHierarchy)
                {
                    uiTextPool[i].gameObject.SetActive(true);
                    return uiTextPool[i];
                }
            }
            //If no available object found, create one, then return it
            GameObject uiText = Instantiate(UIPrefab);
            uiText.transform.SetParent(transform);
            uiText.SetActive(true);
            uiTextPool.Add(uiText);
            return uiText;
        }

        public void ReturnUITextToPool(GameObject uiText)
        {
            uiText.SetActive(false);
            uiText.transform.SetParent(transform);
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}
