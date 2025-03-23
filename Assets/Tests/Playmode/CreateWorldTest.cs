using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SimpleWorldGenerationNS;
using UnityEngine;
using UnityEngine.TestTools;

public class CreateWorldTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void YowhatSimplePasses()
    {
        // Use the Assert class to test conditions
    }


    [SerializeField] private GameObject worldGeneratorPrefab; // Assign in inspector

    [UnityTest]
    public IEnumerator CreateCorrectNumberOfTiles()
    {
        GameObject[] worldGeneratorPreFab = Resources.LoadAll<GameObject>("Prefabs/Generator/WorldGenerator"); // Loads all prefabs
        Assert.IsTrue(worldGeneratorPreFab.Length > 0, "No prefabs found in Resources!");

        GameObject worldGenerator = Object.Instantiate(worldGeneratorPreFab[0], Vector3.zero, Quaternion.identity);
        yield return null;
        Assert.IsTrue(worldGenerator != null, "Failed to instantiate world!");


        WorldGenerationsManager.instance.X = 100;
        WorldGenerationsManager.instance.Y = 100;
        WorldGenerationsManager.instance.Generate();
        yield return null;


        // Find all GameObjects in the hierarchy
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Filter objects whose names start with "Tile"
        GameObject[] tileObjects = allObjects.Where(obj => obj.name.StartsWith("Tile")).ToArray();

        // Validate the number of "Tile" objects matches X * Y
        int expectedTileCount = WorldGenerationsManager.instance.X * WorldGenerationsManager.instance.Y;
        Assert.AreEqual(expectedTileCount, tileObjects.Length, $"Expected {expectedTileCount} tiles, but found {tileObjects.Length}.");

        //set up
        yield return null;
    }


    [UnityTest]
    public IEnumerator Test1()
    {
        GameObject worldGeneratorPreFab = Resources.Load<GameObject>("Prefabs/Generator/WorldGenerator"); // Loads all prefabs
        GameObject worldGenerator = Object.Instantiate(worldGeneratorPreFab, Vector3.zero, Quaternion.identity);
        yield return null;
        Assert.IsTrue(worldGenerator != null, "Failed to instantiate world!");


        WorldGenerationsManager.instance.X = 100;
        WorldGenerationsManager.instance.Y = 100;
        WorldGenerationsManager.instance.Generate();
        yield return null;


        // Find all GameObjects in the hierarchy
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        // Filter objects whose names start with "Tile"
        GameObject[] tileObjects = allObjects.Where(obj => obj.name.StartsWith("Tile")).ToArray();

        // Validate the number of "Tile" objects matches X * Y
        int expectedTileCount = WorldGenerationsManager.instance.X * WorldGenerationsManager.instance.Y;
        Assert.AreEqual(expectedTileCount, tileObjects.Length, $"Expected {expectedTileCount} tiles, but found {tileObjects.Length}.");

        //set up
        yield return null;
    }
}
