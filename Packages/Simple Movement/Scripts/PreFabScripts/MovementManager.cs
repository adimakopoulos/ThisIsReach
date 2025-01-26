using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 

namespace SimpleMovementNS
{
    public class MovementManager : MonoBehaviour
    {
        //[SerializeField]
        //public PersonDependenciesManager dependenciesManager;
        List<NavCellNode> path;
        int currentNode = 0;
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
            if (path !=null ) {
                gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, path[currentNode].position, 1f*Time.deltaTime);
                if (gameObject.transform.position == path[path.Count-1].position)
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

        public void findFood(Vector3 endPos)
        {
            Debug.Log(path);
            path = GlobalPathingService.instance.FindPath(gameObject.transform.position, endPos);
            Debug.Log(path);
        }  
        public void followPath(List<NavCellNode> path)
        {
            Debug.Log(path);
            this.path = path;
            Debug.Log(path);
        }
    }
}
