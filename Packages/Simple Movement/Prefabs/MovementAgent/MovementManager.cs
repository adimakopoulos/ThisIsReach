using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace SimpleMovementNS
{
    public class MovementManager : MonoBehaviour
    {
        //[SerializeField]
        //public PersonDependenciesManager dependenciesManager;
        List<NavCellNode> path;
        int currentNode = 0;
        Vector3 targetlocation;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        //private void OnEnable()
        //{
        //    dependenciesManager.aIManager.onNewDestinationSet += findFood;
        //}
        //private void OnDisable()
        //{
        //    dependenciesManager.aIManager.onNewDestinationSet -= findFood;

        //}
        // Update is called once per frame
        void Update()
        {
            setMovementTargetWhenRightClick();

            if (path != null)
            {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, path[currentNode].position, 1f * Time.deltaTime);
                if (gameObject.transform.position == path[path.Count - 1].position)
                {
                    path = null;
                    currentNode = 0;
                    return;
                }
                if (gameObject.transform.position == path[currentNode].position)
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
                    findFood(hitPoint);
                }
            }
        }

        public void findFood(Vector3 endPos)
        {
            currentNode = 0;
            path = GlobalPathingService.instance.FindPath(gameObject.transform.position, endPos);
            Debug.Log(path);
        }  
        public void followPath(List<NavCellNode> path)
        {
            this.path = path;
            Debug.Log(path);
        }
    }
}
