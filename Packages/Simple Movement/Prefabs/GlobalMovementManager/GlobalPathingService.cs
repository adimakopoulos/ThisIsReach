using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using UnityEngine.Tilemaps;

namespace SimpleMovementNS
{
    public class GlobalPathingService : MonoBehaviour
    {
        // Pathfinding constants
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;


        // Grid properties
        private static Dictionary<GridPosition, NavCellNode> navCells = new Dictionary<GridPosition, NavCellNode>();
        private static Dictionary<Vector3Int, List<NavCellNode>> navCellsPerTile = new Dictionary<Vector3Int, List<NavCellNode>>();
        private List<NavCellNode> openList = new List<NavCellNode>();
        private List<NavCellNode> closedList = new List<NavCellNode>();

        public int width = 10;
        public int height = 10;
        public float navCellDistance = 0.2f; 
        public static GlobalPathingService instance;
        public bool autoGenerateGridOnEnable = false;
        public bool leftClickToBlockNavCell = false;

        public bool createVisualNode = false;
        private List<GameObject> visualNodes = new List<GameObject>();
        // Initialization
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Duplicate of "+this.name+"Instance was found and deleteted");
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            instance = this;
        }

        private void OnEnable()
        {
            if (autoGenerateGridOnEnable)
            {
                CreateGrid();
            }
        }

        private void OnDisable()
        {
            DeleteGrid();
        }

        private void Update()
        {
            LeftClickToBlockNavCell();
        }

        // Grid creation and management
        public void CreateGrid()
        {
            navCells.Clear();
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Vector3 worldPosition = GridToWorldPosition(gridPosition);
                    NavCellNode cellData = new NavCellNode(gridPosition, worldPosition);
                    navCells.Add(gridPosition, cellData);

                    if (createVisualNode) {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = worldPosition;
                        cube.transform.localScale = new Vector3(navCellDistance, 0.05f, navCellDistance);
                        cube.transform.SetParent(this.gameObject.transform);
                        visualNodes.Add(cube);
                    }


                }
            }

            navCellsPerTile.Clear();
            foreach (NavCellNode navCellNode in navCells.Values) {
                Vector3Int flooredNavCellPosition = new Vector3Int(Mathf.FloorToInt(navCellNode.worldPosition.x), 0, Mathf.FloorToInt(navCellNode.worldPosition.z));
                if (navCellsPerTile.ContainsKey(flooredNavCellPosition))
                {
                    navCellsPerTile[flooredNavCellPosition].Add(navCellNode);
                }
                else {
                    List<NavCellNode> list = new List<NavCellNode>() { navCellNode };
                    navCellsPerTile.Add(flooredNavCellPosition, list);
                }

            }

            SetNeighboringListForEveryCell();
            Debug.Log($"Created grid with {navCells.Count} cells");
        }

        public void DeleteGrid()
        {
            navCells.Clear();
            foreach (GameObject cube in visualNodes)
            {
                if (Application.isEditor) {
                    DestroyImmediate(cube);
                }
                else if(Application.isPlaying)
                {
                    Destroy(cube);
                }
            }
            visualNodes.Clear();
            Debug.Log("Navigation grid deleted");
        }

        // Position conversion methods
        public Vector3 GridToWorldPosition(GridPosition gridPosition)
        {
            return new Vector3(
                gridPosition.x * navCellDistance + transform.position.x,
                0,
                gridPosition.z * navCellDistance + transform.position.z
            );
        }

        public GridPosition WorldToGridPosition(Vector3 worldPosition)
        {
            // Calculate raw grid position without rounding
            float rawX = (worldPosition.x - transform.position.x) / navCellDistance;
            float rawZ = (worldPosition.z - transform.position.z) / navCellDistance;

            // Round to nearest grid position
            int x = Mathf.RoundToInt(rawX);
            int z = Mathf.RoundToInt(rawZ);


            return new GridPosition(x, z);
        }

        // Node retrieval methods
        public NavCellNode GetNodeAtGridPosition(GridPosition gridPosition)
        {
            if (navCells.TryGetValue(gridPosition, out NavCellNode node))
            {
                return node;
            }
            return null;
        }

        public NavCellNode GetNodeAtWorldPosition(Vector3 worldPosition)
        {
            GridPosition gridPosition = WorldToGridPosition(worldPosition);
            return GetNodeAtGridPosition(gridPosition);
        }

        // Finds nearest navigation cell to any world position
        public static Vector3 GetClosestNavCellPos(Vector3 worldPosition)
        {
            if (instance == null || navCells.Count == 0)
            {
                Debug.LogWarning("Navigation system not initialized or no cells created");
                return worldPosition;
            }

            GridPosition gridPosition = instance.WorldToGridPosition(worldPosition);
            NavCellNode closestCell = instance.GetNodeAtGridPosition(gridPosition);

            if (closestCell != null)
            {
                return closestCell.worldPosition;
            }

            // If not in grid, find the closest valid cell
            float shortestDistance = float.MaxValue;
            foreach (var cell in navCells.Values)
            {
                float distance = Vector3.Distance(worldPosition, cell.worldPosition);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    closestCell = cell;
                }
            }

            if (closestCell == null)
            {
                Debug.LogWarning("No navigation cells found.");
                return worldPosition;
            }

            return closestCell.worldPosition;
        }

        // Grid validation
        private bool IsValidGridPosition(GridPosition gridPosition)
        {
            return gridPosition.x >= 0 && gridPosition.x < width &&
                   gridPosition.z >= 0 && gridPosition.z < height &&
                   navCells.ContainsKey(gridPosition);
        }

        // Set up neighbors for all cells
        private void SetNeighboringListForEveryCell()
        {
            foreach (var cellPair in navCells)
            {
                List<NavCellNode> neighbourList = new List<NavCellNode>();
                GridPosition gridPos = cellPair.Key;

                // Check all 8 neighboring directions
                for (int xOffset = -1; xOffset <= 1; xOffset++)
                {
                    for (int zOffset = -1; zOffset <= 1; zOffset++)
                    {
                        // Skip the current cell
                        if (xOffset == 0 && zOffset == 0) continue;

                        GridPosition neighborPos = new GridPosition(gridPos.x + xOffset, gridPos.z + zOffset);

                        // Check if this neighbor is within grid bounds
                        if (IsValidGridPosition(neighborPos))
                        {
                            // Add to neighbor list
                            neighbourList.Add(navCells[neighborPos]);
                        }
                    }
                }

                // Assign the neighbors to the cell
                cellPair.Value.neighbourList = neighbourList;
            }
        }

        // Cell manipulation methods
        public bool CanBuildOnTile(Vector3 worldPosition)
        {

            var cellList = navCellsPerTile[new Vector3Int(Mathf.FloorToInt(worldPosition.x), 0, Mathf.FloorToInt(worldPosition.z))];
            foreach (var cell in cellList)
            {
                if (!cell.isWalkable) return false;
            }
            return true;
        }

        private void LeftClickToBlockNavCell()
        {
            if (Input.GetMouseButtonDown(0) && leftClickToBlockNavCell)
            {
                Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (groundPlane.Raycast(ray, out float distance))
                {
                    Vector3 hitPoint = ray.GetPoint(distance);
                    hitPoint.y = 0;
                    Debug.Log("Clicked position on plane: " + hitPoint);
                    BlockNavCell(hitPoint);
                }
            }
        }

        public Vector3 BlockNavCell(Vector3 worldPosition)
        {
            NavCellNode cell = GetNodeAtWorldPosition(worldPosition);
            if (cell != null)
            {
                cell.isWalkable = false;
                GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                marker.transform.position = cell.worldPosition + Vector3.up * 0.1f;
                marker.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                marker.name = "NavBlock_" + cell.gridPosition.ToString();
                Debug.Log($"Nav cell blocked at grid position: {cell.gridPosition}");
                return cell.worldPosition;  
            }
            else
            {
                Debug.LogWarning("No nav cell found at: " + worldPosition);
                return Vector3.zero;
            }
        }

        public void UnblockNavCell(Vector3 worldPosition)
        {
            NavCellNode cell = GetNodeAtWorldPosition(worldPosition);
            if (cell != null)
            {
                cell.isWalkable = true;
                Debug.Log($"Nav cell unblocked at grid position: {cell.gridPosition}");

                // Remove visual debug markers
                GameObject marker = GameObject.Find("NavBlock_" + cell.gridPosition.ToString());
                if (marker != null)
                {
                    DestroyImmediate(marker);
                }
            }
            else
            {
                Debug.LogWarning("No nav cell found at: " + worldPosition);
            }
        }

        public void BlockNavCellAndNeighbors(Vector3 worldPosition)
        {
            NavCellNode cell = GetNodeAtWorldPosition(worldPosition);
            if (cell != null)
            {
                cell.isWalkable = false;

                foreach (NavCellNode neighbor in cell.neighbourList)
                {
                    if (neighbor != null)
                    {
                        neighbor.isWalkable = false;
                    }
                }

                Debug.Log($"Blocked nav cell and neighbors at grid position: {cell.gridPosition}");
            }
            else
            {
                Debug.LogWarning("No nav cell found at: " + worldPosition);
            }
        }

        public void UnblockNavCellAndNeighbors(Vector3 worldPosition)
        {
            NavCellNode cell = GetNodeAtWorldPosition(worldPosition);
            if (cell != null)
            {
                cell.isWalkable = true;

                foreach (NavCellNode neighbor in cell.neighbourList)
                {
                    if (neighbor != null)
                    {
                        neighbor.isWalkable = true;
                    }
                }

                Debug.Log($"Unblocked nav cell and neighbors at grid position: {cell.gridPosition}");
            }
            else
            {
                Debug.LogWarning("No nav cell found at: " + worldPosition);
            }
        }

        public void BlockAllNavCellsOnTile(Vector3 worldPosition)
        {
            GridPosition gridPosition = WorldToGridPosition(worldPosition);
            if (IsValidGridPosition(gridPosition))
            {
                var  cellList = navCellsPerTile[new Vector3Int(Mathf.FloorToInt (worldPosition.x),0,Mathf.FloorToInt (worldPosition.z))];
                foreach (var cell in cellList) { 
                    cell.isWalkable = false;
                }
                Debug.Log($"Blocked nav cell at grid position: {gridPosition}");
            }
            else
            {
                Debug.LogWarning($"No valid grid position found at: {worldPosition}");
            }
        }

        public void UnBlockAllNavCellsOnTile(Vector3 worldPosition)
        {
            GridPosition gridPosition = WorldToGridPosition(worldPosition);
            if (IsValidGridPosition(gridPosition))
            {
                var cellList = navCellsPerTile[new Vector3Int(Mathf.FloorToInt(worldPosition.x), 0, Mathf.FloorToInt(worldPosition.z))];
                foreach (var cell in cellList)
                {
                    cell.isWalkable = false;
                }
                Debug.Log($"Unblocked nav cell at grid position: {gridPosition}");
            }
            else
            {
                Debug.LogWarning($"No valid grid position found at: {worldPosition}");
            }
        }

        // Pathfinding implementation
        public List<NavCellNode> FindPath(Vector3 startWorldPos, Vector3 endWorldPos)
        {
            startWorldPos.y = 0;
            endWorldPos.y = 0;
            NavCellNode startCell = GetNodeAtWorldPosition(startWorldPos);
            NavCellNode endCell = GetNodeAtWorldPosition(endWorldPos);

            if (startCell == null || endCell == null)
            {
                Debug.LogWarning("Start or end position is outside the navigation grid");
                return null;
            }

            if (!endCell.isWalkable)
            {
                Debug.LogWarning("End position is not walkable");
                return null;
            }

            // Initialize pathfinding
            openList = new List<NavCellNode> { startCell };
            closedList = new List<NavCellNode>();

            // Clear previous path data
            foreach (var cell in navCells.Values)
            {
                cell.gCost = float.MaxValue;
                cell.CalculateFCost();
                cell.cameFromNavCellNode = null;
            }

            // Set initial costs
            startCell.gCost = 0;
            startCell.hCost = CalculateDistance(startCell, endCell);
            startCell.CalculateFCost();

            // Main A* loop
            while (openList.Count > 0)
            {
                NavCellNode currentNode = GetLowestFCostNode(openList);

                if (currentNode == endCell)
                {
                    return CalculatePath(endCell);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);

                foreach (var neighborNode in currentNode.neighbourList)
                {
                    if (!neighborNode.isWalkable || closedList.Contains(neighborNode))
                    {
                        continue;
                    }

                    float tentativeGCost = currentNode.gCost + CalculateDistance(currentNode, neighborNode);

                    if (tentativeGCost < neighborNode.gCost)
                    {
                        neighborNode.cameFromNavCellNode = currentNode;
                        neighborNode.gCost = tentativeGCost;
                        neighborNode.hCost = CalculateDistance(neighborNode, endCell);
                        neighborNode.CalculateFCost();

                        if (!openList.Contains(neighborNode))
                        {
                            openList.Add(neighborNode);
                        }
                    }
                }
            }

            Debug.LogWarning("No path found between start and end positions");
            return null;
        }

        private NavCellNode GetLowestFCostNode(List<NavCellNode> nodes)
        {
            NavCellNode lowestFCostNode = nodes[0];
            foreach (var node in nodes)
            {
                if (node.fCost < lowestFCostNode.fCost)
                {
                    lowestFCostNode = node;
                }
                else if (node.fCost == lowestFCostNode.fCost && node.hCost < lowestFCostNode.hCost)
                {
                    lowestFCostNode = node;
                }
            }
            return lowestFCostNode;
        }

        private List<NavCellNode> CalculatePath(NavCellNode endCell)
        {
            List<NavCellNode> path = new List<NavCellNode>();
            path.Add(endCell);

            NavCellNode currentNode = endCell;
            while (currentNode.cameFromNavCellNode != null)
            {
                path.Add(currentNode.cameFromNavCellNode);
                currentNode = currentNode.cameFromNavCellNode;
            }

            path.Reverse();
            return path;
        }

        private float CalculateDistance(NavCellNode a, NavCellNode b)
        {
            int xDistance = Mathf.Abs(a.gridPosition.x - b.gridPosition.x);
            int zDistance = Mathf.Abs(a.gridPosition.z - b.gridPosition.z);
            int remaining = Mathf.Abs(xDistance - zDistance);

            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        // Visualization helpers for debugging
        public void VisualizeGrid()
        {
            foreach (var cell in navCells.Values)
            {
                GameObject visualizer = GameObject.CreatePrimitive(PrimitiveType.Cube);
                visualizer.name = $"GridVis_{cell.gridPosition}";
                visualizer.transform.position = cell.worldPosition + Vector3.up * 0.05f;
                visualizer.transform.localScale = new Vector3(
                    navCellDistance * 0.9f,
                    0.05f,
                    navCellDistance * 0.9f
                );

                Renderer rend = visualizer.GetComponent<Renderer>();
                rend.material.color = cell.isWalkable ?
                    new Color(0, 1, 0, 0.3f) :
                    new Color(1, 0, 0, 0.5f);

                // Make it semi-transparent
                rend.material.color = new Color(
                    rend.material.color.r,
                    rend.material.color.g,
                    rend.material.color.b,
                    0.3f
                );
            }
        }

        public void ClearGridVisualization()
        {
            GameObject[] visualizers = GameObject.FindGameObjectsWithTag("Untagged")
                .Where(go => go.name.StartsWith("GridVis_")).ToArray();

            foreach (var visualizer in visualizers)
            {
                DestroyImmediate(visualizer);
            }
        }

        public bool CanBuildOnNavCell(Vector3 randomPos)
        {
            if (navCells.TryGetValue(WorldToGridPosition(randomPos), out NavCellNode navCellNode))
                return navCellNode.isWalkable;
            return false;
        }
    }
}
