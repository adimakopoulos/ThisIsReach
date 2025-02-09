using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

namespace SimpleMovementNS
{
    public class NavCellNode 
    {

        public float gCost;//how far is from starting node
        public float hCost;//how far is from the end node
        public float fCost;// g + h cost , we want the lowest number. lowest G is closer to Start lowest G is closest to ending
        public Vector3 position;
        public NavCellNode cameFromNavCellNode;
        public int size =1;
        public List<NavCellNode> neighbourList = new List<NavCellNode>();
        public bool isWalkable = true;

        public NavCellNode(Vector3 position)
        {
            this.position = position;
        }

        public override string ToString()
        {
            return "x:" + position.x + " y:" + position.y + " z:"+ position.z;
        }

        internal void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
        public static bool operator == (NavCellNode obj1, NavCellNode obj2)
        {
            if (ReferenceEquals(obj1, null) && ReferenceEquals(obj2, null))
            {
                return true;
            }
            if (ReferenceEquals(obj1, null) || ReferenceEquals(obj2, null))
            {
                return false;
            }
            if (obj1.position.x == obj2.position.x && obj1.position.z == obj2.position.z) { return true; }
                
            return false;
        }
        public static bool operator !=(NavCellNode obj1, NavCellNode obj2)
        {
            return !(obj1 == obj2);
        }
    }
}
