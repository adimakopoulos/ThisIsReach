using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerData playerData;
        public static PlayerManager instance;
        private void Awake()
        {
            instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddResource(ResourceType type, int amount)
        {
            if (ResourceType.WHEAT == type) {
                playerData.food += amount;
                Debug.Log("Increased WHEAT amount by: " + amount);
            }
        }
    }
}