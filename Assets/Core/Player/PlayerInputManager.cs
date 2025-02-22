using SimpleMovementNS;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float ghostAlpha = 0.3f;
    [SerializeField] private LayerMask buildLayer;

    [Header("Prefabs")]
    [SerializeField] private GameObject[] buildingPrefabs;

    private GameObject currentGhost;
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
            Mathf.Floor(position.x ) ,
            0,
            Mathf.Floor(position.z ) 
        );
    }
    #endregion
}
