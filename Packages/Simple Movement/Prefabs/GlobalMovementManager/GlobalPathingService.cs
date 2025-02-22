using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SimpleMovementNS
{
    public class GlobalPathingService : MonoBehaviour
    {

        const int MOVE_STRAIGHT_COST = 10;
        const int MOVE_DIAGONAL_COST = 14;

        private static Dictionary<Vector3, NavCellNode> navCells = new Dictionary<Vector3, NavCellNode>();
        private List<NavCellNode> openList = new List<NavCellNode>();
        private List<NavCellNode> closedList = new List<NavCellNode>();
        public List<NavCellNode> neighboringCells = new List<NavCellNode>();
        public int width=1, height=1;
        public float scaleOfNavigationGrid = 1f;
        public static GlobalPathingService instance;
        public bool autoGenerateGridOnEnable = false;
        public bool leftClickToBlockNavCell = false;
        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }
        
        private void OnEnable()
        {
            if (autoGenerateGridOnEnable) {
                createGrid();
            }
        }
        private void OnDisable()
        {
            deleteGrid();
        }

        // Update is called once per frame
        void Update()
        {
            leftClickToblockNavcell();

        }
        public static Vector3 GetClosestNavCellPos(Vector3 worldPosition)
        {
            NavCellNode closestCell = null;
            float shortestDistance = Mathf.Infinity;

            foreach (var cell in navCells.Values)
            {
                float distance = Vector3.Distance(worldPosition, cell.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestCell = cell;
                }
            }

            if (closestCell == null)
            {
                Debug.LogWarning("No navigation cells found.");
            }

            return closestCell.position;
        }
        private void leftClickToblockNavcell()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!leftClickToBlockNavCell)
                {
                    return;
                }
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                // Create a ray from the main camera to the mouse position
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float distance;

                // Check if the ray intersects the plane
                if (groundPlane.Raycast(ray, out distance))
                {
                    // Calculate the exact hit point
                    Vector3 hitPoint = ray.GetPoint(distance);
                    hitPoint.y = 0;
                    Debug.Log("Clicked position on plane: " + hitPoint);
                    GlobalPathingService.instance.BlockNavCell(hitPoint);
                }
            }
        }

        public bool CanBuildOnTile(Vector3 tilePos3)
        {
            // Filter navCells by matching x and z coordinates
            var matchingCells = navCells.Where(kvp => Mathf.FloorToInt(kvp.Key.x) == tilePos3.x && Mathf.FloorToInt(kvp.Key.z) == tilePos3.z);

            // Check if there are any non-walkable cells among them or no cells at all
            return !matchingCells.Any() || matchingCells.All(cell => cell.Value.isWalkable);
        }
        public bool CanBuildOnNavCell(Vector3 tilePos3)
        {
            // Filter navCells by matching x and z coordinates
            NavCellNode cell = WorldToGridPosition(tilePos3);
            if (cell != null && cell.isWalkable)
            {
                return true;
            }
            else {
                return false;
            }
        }
        /// <summary>
        /// It floors the number to int and it will find all NavCells that start with
        /// that number and block them 
        /// 
        /// </summary>
        /// <param name="tilePos3"></param>
        public void BlockAllNavCellsOnTile(Vector3 tilePos3)
        {
            // Filter navCells by matching x and z coordinates
            var matchingCells = navCells.Where(kvp => Mathf.FloorToInt(kvp.Key.x) == tilePos3.x && Mathf.FloorToInt(kvp.Key.z) == tilePos3.z);

            // Check if there are any cells to block
            if (!matchingCells.Any()) {
                Debug.Log(" No cells found");
            }

            foreach (var cell in matchingCells) { 
                cell.Value.isWalkable = false;
                Debug.Log(" Block all matching cells by setting isWalkable to false finshed");
            }

        }

        public void CreateGrid() {
            createGrid();
        }      
        
        public void DeleteGrid() {
            deleteGrid();
        }
        /// <summary>
        /// Transform the world position to the Closest NavCell
        /// Then Set the isWalkable bool to false
        /// </summary>
        /// <param name="cellPosition"></param>
        public void BlockNavCell(Vector3 cellPosition)
        {
            // Convert Vector3Int to Vector3 since our dictionary keys are Vector3.
            NavCellNode cell = WorldToGridPosition(cellPosition);
            if (cell != null)
            {
                cell.isWalkable = false;
                GameObject a = GameObject.CreatePrimitive(PrimitiveType.Cube);
                a.transform.position = new Vector3(cell.position.x, 0, cell.position.z);
                a.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                Debug.Log("Nav cell blocked at: " + cellPosition);
            }
            else
            {
                Debug.LogWarning("No nav cell found at: " + cellPosition);
            }
        }
        public List<NavCellNode> FindPath(Vector3 start, Vector3 end)
        {

            //--init
            NavCellNode startCell = WorldToGridPosition(start);
            NavCellNode endCell = WorldToGridPosition(end);
            if (!endCell.isWalkable)
            {
                return null;
            }

            openList = new List<NavCellNode>
            {
                startCell
            };
            closedList = new List<NavCellNode>();

            clearPreviousPathsAndCosts();
            initializeCostForFistCell(startCell, endCell);

            while (openList.Count > 0)
            {
                NavCellNode currCellNode = getLowestFcostNode(openList);
                if (currCellNode == endCell)
                {
                    return calculatePath(endCell);
                }

                openList.Remove(currCellNode);
                closedList.Add(currCellNode);
                cycleThroughAllNeigboringTiles(endCell, currCellNode);
            }
    
            //no path
            return null;
        }
        /// <summary>
        /// Finds the closest NavCell and then it set isWalkable to fasle for the navCell and all "touching" Cells
        /// </summary>
        /// <param name="cellPosition"></param>
        public void BlockNavCellAndNeighbors(Vector3 cellPosition)
        {
            NavCellNode cell = WorldToGridPosition(cellPosition);
            if (cell != null)
            {
                // Block the selected cell
                cell.isWalkable = false;

                // Block all neighboring cells
                foreach (NavCellNode neighbor in cell.neighbourList)
                {
                    if (neighbor != null)
                    {
                        neighbor.isWalkable = false;
                    }
                }

                Debug.Log("Blocked nav cell and neighbors at: " + cellPosition);
            }
            else
            {
                Debug.LogWarning("No nav cell found at: " + cellPosition);
            }
        }
        public void UnblockNavCellAndNeighbors(Vector3 cellPosition)
        {
            NavCellNode cell = WorldToGridPosition(cellPosition);
            if (cell != null)
            {
                // Unblock the selected cell
                cell.isWalkable = true;

                // Unblock all neighboring cells
                foreach (NavCellNode neighbor in cell.neighbourList)
                {
                    if (neighbor != null)
                    {
                        neighbor.isWalkable = true;
                    }
                }

                Debug.Log("Unblocked nav cell and neighbors at: " + cellPosition);
            }
            else
            {
                Debug.LogWarning("No nav cell found at: " + cellPosition);
            }
        }
        private void cycleThroughAllNeigboringTiles(NavCellNode endCell, NavCellNode currCellNode)
        {
            foreach (var nCell in currCellNode.neighbourList)
            {
                if (!nCell.isWalkable)
                {
                    closedList.Add(nCell);
                    continue;
                }
                if (closedList.Contains(nCell))
                {
                    continue;
                }
                float tentativeGCost = currCellNode.gCost + calculateDistance(currCellNode, nCell);
                if (tentativeGCost < nCell.gCost)
                {
                    nCell.cameFromNavCellNode = currCellNode;
                    nCell.gCost = tentativeGCost;
                    nCell.hCost = calculateDistance(nCell, endCell);
                    nCell.CalculateFCost();

                    if (!openList.Contains(nCell))
                    {
                        openList.Add(nCell);
                    }
                }
            }
        }
        private void initializeCostForFistCell(NavCellNode startCell, NavCellNode endCell)
        {
            startCell.gCost = 0;
            startCell.hCost = calculateDistance(startCell, endCell);
            startCell.CalculateFCost();
        }
        private void clearPreviousPathsAndCosts()
        {
            foreach (var item in navCells)
            {
                item.Value.gCost = 10_000_000;
                item.Value.CalculateFCost();
                item.Value.cameFromNavCellNode = null;
            }
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
        private float calculateDistance(NavCellNode a, NavCellNode b)
        {
            float xDistance = Mathf.Abs(a.position.x - b.position.x);
            float yDistance = Mathf.Abs(a.position.z - b.position.z);
            float remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }
        private void createGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    NavCellNode cellData = new NavCellNode(new Vector3(adjustOffsetAndScaleX(x), 0, adjustOffsetAndScaleZ(y))); // Create a new NavCellData instance
                    navCells.Add(cellData.position, cellData); // Add to the dictionary
                }
            }
            setNeighboringListForEveryCell();
        }
        private float adjustOffsetAndScaleX(int n)
        {
            return n * scaleOfNavigationGrid + this.gameObject.transform.position.x;
        } 
        private float adjustOffsetAndScaleZ(int n)
        {
            return n * scaleOfNavigationGrid + this.gameObject.transform.position.z;
        }

        private void deleteGrid()
        {
            navCells.Clear();
        }

        private void setNeighboringListForEveryCell()
        {
            //setNeighbourList
            foreach (var cell in navCells)
            {
                List<NavCellNode> neighbourList = new List<NavCellNode>();
                if (leftSideOutOfBounds(cell))
                {
                    //left
                    neighbourList.Add(navCells[new Vector3(cell.Value.position.x - adjustOffsetAndScaleX(1), 0, cell.Value.position.z)]);
                    //leftDown
                    if (cell.Value.position.z - adjustOffsetAndScaleZ(1) >= 0 + gameObject.transform.position.z)
                        neighbourList.Add(navCells[new Vector3(cell.Value.position.x - adjustOffsetAndScaleX(1), 0, cell.Value.position.z - adjustOffsetAndScaleZ(1))]);
                    //leftUp
                    if (cell.Value.position.z + adjustOffsetAndScaleZ(1) < adjustOffsetAndScaleZ(height))
                        neighbourList.Add(navCells[new Vector3(cell.Value.position.x - adjustOffsetAndScaleX(1), 0, cell.Value.position.z + adjustOffsetAndScaleZ(1))]);

                }
                if (rightSideOutOfBounds(cell))
                {
                    //right
                    neighbourList.Add(navCells[new Vector3(cell.Value.position.x + adjustOffsetAndScaleX(1), 0, cell.Value.position.z)]);

                    //rightDown
                    if (cell.Value.position.z - adjustOffsetAndScaleZ(1) >= 0 + gameObject.transform.position.z)
                        neighbourList.Add(navCells[new Vector3(cell.Value.position.x + adjustOffsetAndScaleX(1), 0, cell.Value.position.z - adjustOffsetAndScaleZ(1))]);
                    //rightUp
                    if (cell.Value.position.z + adjustOffsetAndScaleZ(1) < adjustOffsetAndScaleZ(height))
                        neighbourList.Add(navCells[new Vector3(cell.Value.position.x + adjustOffsetAndScaleX(1), 0, cell.Value.position.z + adjustOffsetAndScaleZ(1))]);
                }
                if (bottomSideOutOfBounds(cell))
                {
                    //down
                    neighbourList.Add(navCells[new Vector3(cell.Value.position.x, 0, cell.Value.position.z - adjustOffsetAndScaleZ(1))]);
                }
                if (TopSideOutOfBounds(cell))
                {
                    //up
                    neighbourList.Add(navCells[new Vector3(cell.Value.position.x, 0, cell.Value.position.z + adjustOffsetAndScaleZ(1))]);

                }
                cell.Value.neighbourList = neighbourList;
            }
        }

        private bool TopSideOutOfBounds(KeyValuePair<Vector3, NavCellNode> cell)
        {
            return cell.Value.position.z + adjustOffsetAndScaleZ(1) < adjustOffsetAndScaleZ(height);
        }

        private bool bottomSideOutOfBounds(KeyValuePair<Vector3, NavCellNode> cell)
        {
            return cell.Value.position.z - adjustOffsetAndScaleZ(1) >= 0 + gameObject.transform.position.z;
        }

        private bool rightSideOutOfBounds(KeyValuePair<Vector3, NavCellNode> cell)
        {
            return cell.Value.position.x + adjustOffsetAndScaleX(1) < adjustOffsetAndScaleX(width);
        }

        private bool leftSideOutOfBounds(KeyValuePair<Vector3, NavCellNode> cell)
        {
            return cell.Value.position.x - adjustOffsetAndScaleX(1) >= 0 + gameObject.transform.position.x;
        }

        private static NavCellNode WorldToGridPosition(Vector3 worldPosition)
        {
            NavCellNode closestCell = null;
            float shortestDistance = Mathf.Infinity;

            foreach (var cell in navCells.Values)
            {
                float distance = Vector3.Distance(worldPosition, cell.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestCell = cell;
                }
            }

            if (closestCell == null)
            {
                Debug.LogWarning("No navigation cells found.");
            }

            return closestCell;
        }

        private void OnDrawGizmosSelected()
        {
            // If navCells is null or empty, do nothing.
            if (navCells == null || navCells.Count == 0)
                return;

            // Get the current camera (Scene view camera)
            Camera cam = Camera.current;
            if (cam == null)
                return;

            // Define the distance threshold within which gizmos will be drawn.
            // You can adjust this value in the inspector if you expose it as a public field.
            const float gizmos_DRAW_THRESHOLD = 50f;
            float thresholdSqr = gizmos_DRAW_THRESHOLD * gizmos_DRAW_THRESHOLD;

            // Optionally, determine whether to draw labels based on a count threshold.
            const int LABEL_DRAW_COUNT_THRESHOLD = 5000;
            bool drawLabels = navCells.Count <= LABEL_DRAW_COUNT_THRESHOLD;

            // Set a base color for the gizmos.
            Gizmos.color = Color.green;

            // Loop through each navigation cell
            foreach (var cell in navCells.Values)
            {
                // Calculate the squared distance from the cell to the camera.
                float distanceSqr = (cell.position - cam.transform.position).sqrMagnitude;

                // Skip this cell if it is beyond the threshold.
                if (distanceSqr > thresholdSqr)
                    continue;

                // Draw a sphere for the navigation point.
                Gizmos.DrawSphere(cell.position, 0.01f);

                // Draw labels if allowed.
                if (drawLabels)
                {
                    string labelText = string.Format("f:{0}\ng:{1}\nh:{2}", cell.fCost, cell.gCost, cell.hCost);
                    if (cell.isWalkable)
                    {
                        labelText = string.Format("x{3},z{4}\n f:{0}\ng:{1}\nh:{2}WW", cell.fCost, cell.gCost, cell.hCost,cell.position.x,cell.position.z);

                    }
                    else
                    {
                        labelText = string.Format("x{3},z{4}\n f:{0}\ng:{1}\nh:{2}xx", cell.fCost, cell.gCost, cell.hCost, cell.position.x, cell.position.z);

                    }

                    Handles.Label(cell.position, labelText);
                }
            }
        }



 
    }
}
