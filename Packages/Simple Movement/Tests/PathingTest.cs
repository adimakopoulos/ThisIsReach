using NUnit.Framework;
using SimpleMovementNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TextCore.Text;

public class PathingTest
{
    [UnityTest]
    public IEnumerator PathingTestWithEnumeratorPasses()
    {
        GameObject managers = new GameObject();
        managers.name = "managers";
        managers.transform.position = new Vector3(0, 0, 0);
        GlobalPathingService globalPathingService = managers.AddComponent<GlobalPathingService>();
        globalPathingService.width = 10;
        globalPathingService.height = 10;
        globalPathingService.scaleOfNavigationGrid = 1;

        globalPathingService.CreateGrid();


        //GameObject searchingForFood = Resources.Load("Person") as GameObject;
        GameObject prefab = Resources.Load<GameObject>("Person");
        GameObject searchingForFood = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        searchingForFood.name = "searchingForFood";
        searchingForFood.transform.position = new Vector3(0, 0, 0);

        //then
        List<NavCellNode> path = managers.GetComponent<GlobalPathingService>().FindPath(searchingForFood.transform.position, new Vector3(9, 0, 1));
        searchingForFood.GetComponent<MovementManager>().followPath(path);

        Debug.Assert(searchingForFood != null, "searchingForFood != null");
        yield return new WaitForSeconds(10);
        Assert.That(searchingForFood != null);
        yield return null;
    }

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
        Assert.DoesNotThrow(() =>
        {
            globalPathingService.CreateGrid();
            globalPathingService.DeleteGrid();
        });
        yield return null;

    }


    [UnityTest]
    public IEnumerator CreateGridWithOffset()
    {
        GameObject managers = new GameObject();
        managers.name = "managers";
        managers.transform.position = new Vector3(1.1f, 0, 1.1f);
        GlobalPathingService globalPathingService = managers.AddComponent<GlobalPathingService>();
        globalPathingService.width = 10;
        globalPathingService.height = 10;
        globalPathingService.scaleOfNavigationGrid = 0.1f;
        Assert.DoesNotThrow(() =>
        {
            globalPathingService.CreateGrid();
            globalPathingService.DeleteGrid();
        });

        yield return null;

    }



}


