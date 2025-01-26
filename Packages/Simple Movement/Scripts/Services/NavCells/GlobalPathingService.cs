using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace SimpleMovementNS
{
    public class GlobalPathingService : MonoBehaviour
    {

        const int MOVE_STRAIGHT_COST = 10;
        const int MOVE_DIAGONAL_COST = 14;

        private Dictionary<Vector3, NavCellNode> navCells = new Dictionary<Vector3, NavCellNode>();
        private List<NavCellNode> openList = new List<NavCellNode>();
        private List<NavCellNode> closedList = new List<NavCellNode>();
        public List<NavCellNode> neighboringCells = new List<NavCellNode>();
        public int width, height;
        public static GlobalPathingService instance;
        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }
        
        private void OnEnable()
        {
            
        }
        private void OnDisable()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void CreateGrid() {
            createGrid();
        }
        public List<NavCellNode> FindPath(Vector3 start, Vector3 end)
        {

            //--init
            NavCellNode startCell = navCells[new Vector3(Mathf.FloorToInt(start.x), Mathf.FloorToInt(start.y), Mathf.FloorToInt(start.z))];
            NavCellNode endCell = navCells[end];
            openList = new List<NavCellNode>
            {
                startCell
            };
            closedList = new List<NavCellNode>();

            foreach (var item in navCells)
            {
                item.Value.gCost = int.MaxValue;
                item.Value.CalculateFCost();
                item.Value.cameFromNavCellNode = null;
            }
            startCell.gCost = 0;
            startCell.hCost = calculateDistance(startCell, navCells[end]);
            startCell.CalculateFCost();

            //--cycle
            while (openList.Count > 0)
            {
                NavCellNode currCellNode = getLowestFcostNode(openList);
                if (currCellNode == endCell)
                {
                    return calculatePath(endCell);
                }

                openList.Remove(currCellNode);
                closedList.Remove(currCellNode);

                foreach (var nCell in currCellNode.neighbourList)
                {
                    if (closedList.Contains(nCell)) {
                        continue;
                    }
                    int tentativeGCost= currCellNode.gCost+ calculateDistance(currCellNode, nCell);
                    if (tentativeGCost < nCell.gCost) {
                        nCell.cameFromNavCellNode = currCellNode;
                        nCell.gCost = tentativeGCost;
                        nCell.hCost = calculateDistance(nCell, endCell);
                        nCell.CalculateFCost();

                        if (!openList.Contains(nCell)) { 
                            openList.Add(nCell);
                        }
                    }
                }
            }
            //no path
            return null;
        }

        private List<NavCellNode> calculatePath(NavCellNode endCell)
        {
            List<NavCellNode>finalePath = new List<NavCellNode>();
            finalePath.Add(endCell);
            NavCellNode currentNode = endCell;
            while (currentNode.cameFromNavCellNode != null) {
                finalePath.Add(currentNode.cameFromNavCellNode);
                currentNode = currentNode.cameFromNavCellNode;
            }
            finalePath.Reverse();
            return finalePath;
        }

        private NavCellNode getLowestFcostNode(List<NavCellNode> nodes)
        {
            NavCellNode lowestFcostNode = nodes[0];
            nodes.ForEach(n =>
            {
                lowestFcostNode = n.fCost < lowestFcostNode.fCost ? n : lowestFcostNode;
            });
            return lowestFcostNode;
        }
        private int calculateDistance(NavCellNode a, NavCellNode b)
        {
            int xDistance = Mathf.Abs(a.position.x - b.position.x);
            int yDistance = Mathf.Abs(a.position.z - b.position.z);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }



        private void createGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    NavCellNode cellData = new NavCellNode(new Vector3Int(x, 0, y)); // Create a new NavCellData instance
                    navCells.Add(cellData.position, cellData); // Add to the dictionary
                }
            }

            setNeighboringListForEveryCell();
        }

        private void setNeighboringListForEveryCell()
        {
            //setNeighbourList
            foreach (var cell in navCells)
            {
                List<NavCellNode> neighbourList = new List<NavCellNode>();
                if (cell.Value.position.x - 1 >= 0)
                {
                    //left
                    neighbourList.Add(navCells[new Vector3(cell.Value.position.x - 1, 0, cell.Value.position.z)]);
                    //leftDown
                    if (cell.Value.position.z - 1 >= 0)
                        neighbourList.Add(navCells[new Vector3(cell.Value.position.x - 1, 0, cell.Value.position.z - 1)]);
                    //leftUp
                    if (cell.Value.position.z + 1 < height)
                        neighbourList.Add(navCells[new Vector3(cell.Value.position.x - 1, 0, cell.Value.position.z + 1)]);

                }
                if (cell.Value.position.x + 1 < width)
                {
                    //right
                    neighbourList.Add(navCells[new Vector3(cell.Value.position.x + 1, 0, cell.Value.position.z)]);

                    //rightDown
                    if (cell.Value.position.z - 1 >= 0)
                        neighbourList.Add(navCells[new Vector3(cell.Value.position.x + 1, 0, cell.Value.position.z - 1)]);
                    //rightUp
                    if (cell.Value.position.z + 1 < height)
                        neighbourList.Add(navCells[new Vector3(cell.Value.position.x + 1, 0, cell.Value.position.z + 1)]);
                }
                if (cell.Value.position.z - 1 >= 0)
                {

                    //down
                    neighbourList.Add(navCells[new Vector3(cell.Value.position.x, 0, cell.Value.position.z - 1)]);


                }
                if (cell.Value.position.z + 1 < height)
                {
                    //up
                    neighbourList.Add(navCells[new Vector3(cell.Value.position.x , 0, cell.Value.position.z + 1)]);

                }
                cell.Value.neighbourList = neighbourList;
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var item in navCells)
            {
                Gizmos.DrawSphere(item.Value.position, 0.1f);
                Handles.Label(item.Value.position, item.Value.ToString());
                Handles.Label(item.Value.position, "\n hCost" + item.Value.hCost);
                Handles.Label(item.Value.position, "\n\n\n gCost" + item.Value.gCost);
                Handles.Label(item.Value.position, "\n\n\n\n\n fCost" + item.Value.fCost);
            }
        }
    }
}
