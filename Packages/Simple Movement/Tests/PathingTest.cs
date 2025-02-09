using NUnit.Framework;
using SimpleMovementNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TextCore.Text;

public class PathingTest
{


    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PathingTestWithEnumeratorPasses()
    {
        GameObject managers = new GameObject();
        managers.name = "managers";
        managers.transform.position = new Vector3(0,0,0);
        managers.AddComponent<GlobalPathingService>();
        managers.GetComponent<GlobalPathingService>().width = 10;
        managers.GetComponent<GlobalPathingService>().height = 10;
        managers.GetComponent<GlobalPathingService>().CreateGrid();


        //GameObject searchingForFood = Resources.Load("Person") as GameObject;
        GameObject prefab = Resources.Load<GameObject>("Person");
        GameObject searchingForFood = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        searchingForFood.name = "searchingForFood";
        searchingForFood.transform.position = new Vector3(0.2f,0,1.2f);

        //then
        List<NavCellNode> path = managers.GetComponent<GlobalPathingService>().FindPath(searchingForFood.transform.position, new Vector3(9, 0, 1));
        searchingForFood.GetComponent<MovementManager>().followPath(path);

        Debug.Assert(searchingForFood != null, "searchingForFood != null");
        yield return new WaitForSeconds(10);
        Assert.That(searchingForFood != null);
        yield return null;
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator CreateGrid()
    {
        GameObject managers = new GameObject();
        managers.name = "managers";
        managers.transform.position = new Vector3(0, 0, 0);
        GlobalPathingService globalPathingService = managers.AddComponent<GlobalPathingService>();
        globalPathingService.width = 2;
        globalPathingService.height = 2;
        globalPathingService.scaleOfNavigationGrid = 0.5f;
        Assert.DoesNotThrow(() => {
            globalPathingService.CreateGrid();
        });
        yield return null;

    }
}
