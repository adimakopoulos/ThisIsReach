
// Navigation cell node with both grid and world positions
using System.Collections.Generic;
using UnityEngine;

namespace SimpleMovementNS
{
    public class NavCellNode
    {
        public GridPosition gridPosition;
        public Vector3 worldPosition;
        public bool isWalkable = true;
        public float gCost = float.MaxValue;
        public float hCost;
        public float fCost;
        public NavCellNode cameFromNavCellNode;
        public List<NavCellNode> neighbourList = new List<NavCellNode>();

        public NavCellNode(GridPosition gridPosition, Vector3 worldPosition)
        {
            this.gridPosition = gridPosition;
            this.worldPosition = worldPosition;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }
}