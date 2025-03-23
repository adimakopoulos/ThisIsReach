//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace SimpleMovementNS
//{
//    public class AStarHelper 
//    {
//        public static List<Vector3> AStar(Vector3 start, Vector3 goal)
//        {
//            List<Vector3> path = new List<Vector3>();

//            // Define a priority queue for open set
//            PriorityQueue<Vector3> openSet = new PriorityQueue<Vector3>();

//            // Dictionary to keep track of the parent node of each cell
//            Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();

//            // Dictionary to store the cost of reaching each cell from the start cell
//            Dictionary<Vector3, float> gScore = new Dictionary<Vector3, float>();

//            // Dictionary to store the estimated total cost of reaching each cell (gScore + heuristic)
//            Dictionary<Vector3, float> fScore = new Dictionary<Vector3, float>();

//            // Initialize gScore and fScore for the start cell
//            gScore[start] = 0f;
//            fScore[start] = HeuristicCostEstimate(start, goal);

//            // Add the start cell to the open set
//            openSet.Enqueue(start, fScore[start]);

//            while (openSet.Count > 0)
//            {
//                // Get the cell with the lowest fScore from the open set
//                Vector3 current = openSet.Dequeue();

//                // If the current cell is the goal, reconstruct the path and return it
//                if (current == goal)
//                {
//                    path = ReconstructPath(cameFrom, current);
//                    return path;
//                }

//                // Loop through neighbors of the current cell
//                foreach (Vector3 neighbor in GetNeighbors(current))
//                {
//                    // Calculate the tentative gScore for the neighbor
//                    float tentativeGScore = gScore[current] + CostToMove(current, neighbor);

//                    // If the tentative gScore is lower than the current gScore for the neighbor, update it
//                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
//                    {
//                        // Update the cameFrom dictionary
//                        cameFrom[neighbor] = current;

//                        // Update gScore and fScore for the neighbor
//                        gScore[neighbor] = tentativeGScore;
//                        fScore[neighbor] = gScore[neighbor] + HeuristicCostEstimate(neighbor, goal);

//                        // If the neighbor is not in the open set, add it
//                        if (!openSet.Contains(neighbor))
//                            openSet.Enqueue(neighbor, fScore[neighbor]);
//                    }
//                }
//            }

//            // If no path is found, return an empty list
//            return path;
//        }

//        // Calculate the heuristic cost estimate (Manhattan distance)
//        private static float HeuristicCostEstimate(Vector3 from, Vector3 to)
//        {
//            return Mathf.Abs(to.x - from.x) + Mathf.Abs(to.z - from.z);
//        }

//        // Get the neighbors of a given cell
//        private static List<Vector3> GetNeighbors(Vector3 cell)
//        {
//            List<Vector3> neighbors = new List<Vector3>();

//            // Assuming movement is allowed in four directions: up, down, left, right
//            neighbors.Add(new Vector3(cell.x + 1, 0, cell.z));
//            neighbors.Add(new Vector3(cell.x - 1, 0, cell.z));
//            neighbors.Add(new Vector3(cell.x, 0, cell.z + 1));
//            neighbors.Add(new Vector3(cell.x, 0, cell.z - 1));

//            return neighbors;
//        }

//        // Calculate the cost to move from one cell to another (assuming constant cost for all movements)
//        private static float CostToMove(Vector3 from, Vector3 to)
//        {
//            return 1f; // Assuming constant cost for all movements
//        }

//        // Reconstruct the path from start to goal using the cameFrom dictionary
//        private static List<Vector3> ReconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 current)
//        {
//            List<Vector3> path = new List<Vector3>();

//            while (cameFrom.ContainsKey(current))
//            {
//                path.Insert(0, current); // Insert the current cell at the beginning of the path
//                current = cameFrom[current]; // Move to the parent cell
//            }

//            path.Insert(0, current); // Insert the start cell at the beginning of the path

//            return path;
//        }


//    }





//    public class PriorityQueue<T>
//    {
//        private SortedDictionary<float, Queue<T>> queueDict = new SortedDictionary<float, Queue<T>>();

//        public void Enqueue(T item, float priority)
//        {
//            if (!queueDict.ContainsKey(priority))
//            {
//                queueDict[priority] = new Queue<T>();
//            }
//            queueDict[priority].Enqueue(item);
//        }

//        public T Dequeue()
//        {
//            // Find the queue with the highest priority
//            var highestPriorityQueue = queueDict.First().Value;

//            // Dequeue an item from the highest priority queue
//            var item = highestPriorityQueue.Dequeue();

//            // Remove the queue if it's empty
//            if (highestPriorityQueue.Count == 0)
//            {
//                queueDict.Remove(queueDict.First().Key);
//            }

//            return item;
//        }

//        public bool Contains(T item)
//        {
//            return queueDict.Any(kv => kv.Value.Contains(item));
//        }

//        public int Count
//        {
//            get { return queueDict.Values.Sum(queue => queue.Count); }
//        }
//    }


//}
