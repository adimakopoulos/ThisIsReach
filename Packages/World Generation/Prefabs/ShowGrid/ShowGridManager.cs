using SimpleMovementNS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleWorldGenerationNS
{
    public class ShowGridManager : MonoBehaviour
    {
        private int gridWidth = 10;
        private int gridHeight = 10;
        private float cellSize = 1.0f;
        public Material lineMaterial;
        public float lineWidth = 0.05f;

        private List<LineRenderer> lineRenderers = new List<LineRenderer>();

        void Start()
        {
            WorldGenerationsManager worldGenerationsManager = GetComponent<WorldGenerationsManager>();
            gridWidth = worldGenerationsManager.X;
            gridHeight = worldGenerationsManager.Y;
            cellSize = worldGenerationsManager.widthOfTile;
            if (lineMaterial == null)
            {
                lineMaterial = Resources.Load<Material>("LineMat"); ;
            }
            DrawGrid();
        }

        void DrawGrid()
        {
            for (int x = 0; x <= gridWidth; x++)
            {
                CreateLine(new Vector3(x * cellSize + gameObject.transform.position.x, 0, 0 + gameObject.transform.position.z), new Vector3(x * cellSize + gameObject.transform.position.x, 0, gridHeight * cellSize + gameObject.transform.position.z));
            }

            for (int z = 0; z <= gridHeight; z++)
            {
                CreateLine(new Vector3(0 + gameObject.transform.position.x, 0, z * cellSize + gameObject.transform.position.z), new Vector3(gridWidth * cellSize + gameObject.transform.position.z, 0, z * cellSize + gameObject.transform.position.z));
            }
        }

        void CreateLine(Vector3 start, Vector3 end)
        {
            GameObject lineObj = new GameObject("GridLine");
            lineObj.transform.parent = this.transform;
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            lineRenderers.Add(lr);
        }
    }
}