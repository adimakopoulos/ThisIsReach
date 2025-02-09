using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThisIsReach
{
    public class ShowGridManager : MonoBehaviour
    {
        public int gridWidth = 10;
        public int gridHeight = 10;
        public float cellSize = 1.0f;
        public Color gridColor = Color.green;

        private void OnDrawGizmos()
        {
            Gizmos.color = gridColor;
            DrawGrid();
        }

        private void DrawGrid()
        {
            for (int x = 0; x <= gridWidth; x++)
            {
                Vector3 startPoint = new Vector3(x * cellSize, 0, 0);
                Vector3 endPoint = new Vector3(x * cellSize, 0, gridHeight * cellSize);
                Gizmos.DrawLine(startPoint, endPoint);
            }

            for (int z = 0; z <= gridHeight; z++)
            {
                Vector3 startPoint = new Vector3(0, 0, z * cellSize);
                Vector3 endPoint = new Vector3(gridWidth * cellSize, 0, z * cellSize);
                Gizmos.DrawLine(startPoint, endPoint);
            }
        }
    }
}
