using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLocationService : MonoBehaviour
{
    public static RandomLocationService instance;
    public static RandomLocationService GetRandomLocationService()
    {
        if (instance == null)
            Debug.LogError("no instance for Singleton RandomLocationService");

        return instance;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
