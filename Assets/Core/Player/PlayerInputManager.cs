using SimpleMovementNS;
using UnityEngine;
using UnityEngine.UIElements;
using ThisIsReach;

namespace ThisIsReach
{
    public class BuildingManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float ghostAlpha = 0.3f;

        [Header("Prefabs")]
        [SerializeField] private GameObject[] buildingPrefabs;

        private GameObject currentGhost;
        private GameObject SelectedBuilding;
        [SerializeField] private GameObject selectionEffect;

        public LayerMask buildingLayer; // Layer for buildings
        private MaterialPropertyBlock mpb;

        #region Unity Lifecycle
        void Awake()
        {
            mpb = new MaterialPropertyBlock();
        }

        void Update()
        {
            HandleBuildingSelection();
            UpdateGhostPosition();
            HandlePlacementInput();
        }
        #endregion

        #region Core Functionality
        void StartBuilding(int prefabIndex)
        {
            if (currentGhost != null) Destroy(currentGhost);

            currentGhost = Instantiate(buildingPrefabs[prefabIndex]);
            SetGhostTransparency(ghostAlpha);
        }

        void SetGhostTransparency(float alpha)
        {
            foreach (var renderer in currentGhost.GetComponentsInChildren<Renderer>())
            {
                renderer.GetPropertyBlock(mpb);
                mpb.SetColor("_BaseColor", GetOriginalColor(renderer, alpha));
                renderer.SetPropertyBlock(mpb);
            }
        }

        Color GetOriginalColor(Renderer renderer, float alpha)
        {
            Color original = renderer.sharedMaterial.color;
            return new Color(original.r, original.g, original.b, alpha);
        }
        #endregion

        #region Input Handling
        void HandleBuildingSelection()
        {
            for (int i = 0; i < buildingPrefabs.Length; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    StartBuilding(i);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                {
                    // Check if a ghost building exists
                    if (currentGhost != null)
                    {
                        return; // Do nothing if a ghost building is present
                    }

                    // Perform a raycast from the camera to the mouse position
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingLayer))
                    {
                        // Check if the hit object has a Building component or tag
                        Transform colliderBuildingLayer = hit.collider.GetComponent<Transform>();
                        Debug.Log(colliderBuildingLayer.gameObject.name);
                        if (colliderBuildingLayer != null)
                        {
                            if (SelectedBuilding != null) {
                                deselectLastBuilding();
                            }
                            IBuilding building = colliderBuildingLayer.GetComponentInParent<IBuilding>();
                            building.SelecteBuilding();
                            Debug.Log(" selectionEffect.gameObject.SetActive(true);");
                            selectionEffect.gameObject.SetActive(true);
                            selectionEffect.transform.position = colliderBuildingLayer.gameObject.transform.position;
                            SelectedBuilding = colliderBuildingLayer.gameObject;
                        }

                    }
                    else
                    {
                        if (SelectedBuilding == null)
                        {
                            return;
                        }
                        deselectLastBuilding();
                    }
                }
            }
        }

        private void deselectLastBuilding()
        {
            Debug.Log(" selectionEffect.gameObject.SetActive(false);");
            IBuilding building = SelectedBuilding?.GetComponentInParent<IBuilding>();
            building?.DeselecteBuilding();
            selectionEffect.gameObject.SetActive(false);
            SelectedBuilding = null;
        }

        void HandlePlacementInput()
        {
            if (!currentGhost) return;

            if (Input.GetMouseButtonDown(1)) CancelBuilding();
            if (Input.GetMouseButtonDown(0)) TryPlaceBuilding();
        }
        #endregion

        #region Placement Logic
        void TryPlaceBuilding()
        {
            if (!HasValidPosition()) return;

            GlobalPathingService.instance.BlockAllNavCellsOnTile(currentGhost.transform.position);
            Instantiate(currentGhost, currentGhost.transform.position, Quaternion.identity);
            Destroy(currentGhost);
        }

        void CancelBuilding()
        {
            Destroy(currentGhost);
        }

        bool HasValidPosition()
        {
            return GlobalPathingService.instance.CanBuildOnTile(currentGhost.transform.position);
        }
        #endregion

        #region Ghost Positioning
        void UpdateGhostPosition()
        {
            if (!currentGhost) return;
            currentGhost.transform.position = SnapToGrid(RaycastHelper.mousePositionToWorld());
        }

        Vector3 SnapToGrid(Vector3 position)
        {

            return new Vector3(
                Mathf.Floor(position.x),
                0,
                Mathf.Floor(position.z)
            );
        }
        #endregion
    }

}