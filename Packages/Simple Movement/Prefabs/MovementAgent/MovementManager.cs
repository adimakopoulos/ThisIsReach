using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace SimpleMovementNS
{
    public class MovementManager : MonoBehaviour
    {

        List<NavCellNode> path;
        int currentNode = 0;
        Vector3 targetlocation;
        void Start()
        {
        
        }

        void Update()
        {
            setMovementTargetWhenRightClick();

            if (path != null)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, path[currentNode].worldPosition, 5f * Time.deltaTime);

                if (gameObject.transform.position == path[path.Count - 1].worldPosition)
                {
                    path = null;
                    currentNode = 0;
                    return;
                }
                if (gameObject.transform.position == path[currentNode].worldPosition)
                {
                    currentNode++;
                }

            }

        }

        private Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        private void setMovementTargetWhenRightClick()
        {
            if (Input.GetMouseButtonDown(1))
            {
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
                    ResolvePath(hitPoint);
                }
            }
        }

        public void ResolvePath(Vector3 endPos)
        {
            currentNode = 0;
            path = GlobalPathingService.instance.FindPath(gameObject.transform.position, endPos);
        }
        public void ResolvePathIgnoringNavCells(Vector3 endPos)
        {
            currentNode = 0;
           // path = new List<SimpleMovementNSv2.GlobalPathingServiceV2.NavCellNode>() { new SimpleMovementNSv2.GlobalPathingServiceV2.NavCellNode(transform.position), new SimpleMovementNSv2.GlobalPathingServiceV2.NavCellNode(endPos) };
        }
        public void followPath(List<NavCellNode> path)
        {
            this.path = path;
        }
    }
}
