using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ThisIsReach
{
    [RequireComponent(typeof(TMP_Text))]
    public class ResourceCounter : MonoBehaviour
    {
        private TMP_Text counterText;
        private int currentResources;

        void Start()
        {
            counterText = GetComponent<TMP_Text>();
            UpdateDisplay(0); // Initialize with zero
        }

        public void AddResources(int amount)
        {
            currentResources += amount;
            UpdateDisplay(amount);
        }

        void UpdateDisplay(int delta)
        {
            counterText.text = $"Resources: {currentResources} (+{delta})";
            //ounterText.GetComponent<Animator>().Play("Pulse");
        }
    }

}
