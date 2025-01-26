using System.Linq;
using ThisIsReach;
using UnityEditor;
using UnityEngine;

public class DeveloperCheatsMenu : EditorWindow
{

    //-----------------THIRST-----------------
    [MenuItem("Developer Cheats/Person Cheats/Add Need THIRST", true)] // Note the 'true' parameter
    static bool ValidateAddNeedTHIRST()
    {
        return Application.isPlaying; // Ensures the menu item is available only during play mode
    }


    [MenuItem("Developer Cheats/Person Cheats/Add Need THIRST")]
    static void AddNeedTHIRST()
    {

        // Search for the first GameObject called "Person"
        GameObject person = GameObject.Find("Person");

        // If the person GameObject is found, add need or perform some action
        if (person != null)
        {
            person.GetComponent<NeedsManager>()?.EnableNeed(new NeedData(new MinMaxCurrWarnTrackerData(0, 20, 20, 5, 15), NeedEnum.THIRST));
            
            // Add your logic here
            Debug.Log("Need added to person!");
        }
        else
        {
            Debug.LogWarning("Person GameObject not found!");
        }
    }


    //-----------------HUNGER-----------------
    [MenuItem("Developer Cheats/Person Cheats/Add Need HUNGER", true)] // Note the 'true' parameter
    static bool ValidateAddNeedHUNGER()
    {
        return Application.isPlaying; // Ensures the menu item is available only during play mode
    }


    [MenuItem("Developer Cheats/Person Cheats/Add Need HUNGER")]
    static void AddNeedHUNGER()
    {

        // Search for the first GameObject called "Person"
        GameObject person = GameObject.Find("Person");

        // If the person GameObject is found, add need or perform some action
        if (person != null)
        {
            person.GetComponent<NeedsManager>()?.EnableNeed(new NeedData (new MinMaxCurrWarnTrackerData(0,20,20,5,15),NeedEnum.HUNGER));

            // Add your logic here
            Debug.Log("AddNeedHUNGER: Need added to person!");
        }
        else
        {
            Debug.LogWarning("Person GameObject not found!");
        }
    } 
    
    //-----------------HUNGER-----------------
    [MenuItem("Developer Cheats/Person Cheats/set destination", true)] // Note the 'true' parameter
    static bool ValidateSetDestination()
    {
        return Application.isPlaying; // Ensures the menu item is available only during play mode
    }


    [MenuItem("Developer Cheats/Person Cheats/set destination")]
    static void AddnewDestination()
    {

        // Search for the first GameObject called "Person"
        GameObject person = GameObject.Find("Person");

        // If the person GameObject is found, add need or perform some action
        if (person != null)
        {
            person.GetComponent<NeedsManager>()?.EnableNeed(new NeedData (new MinMaxCurrWarnTrackerData(0,20,20,5,15),NeedEnum.HUNGER));

            // Add your logic here
            Debug.Log("AddNeedHUNGER: Need added to person!");
        }
        else
        {
            Debug.LogWarning("Person GameObject not found!");
        }
    }
}
