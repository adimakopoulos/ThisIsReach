using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

[EditorTool("Tile Brush Tool")]
public class TileBrushTool : EditorTool
{
    // Optionally assign an icon for the tool in the inspector
    [SerializeField] private Texture2D m_ToolIcon;

    public override GUIContent toolbarIcon
    {
        get
        {
            GUIContent iconContent = new GUIContent
            {
                text = "Tile Brush",
                image = m_ToolIcon,
                tooltip = "Tile Brush Tool\nLeft-click in the Scene to paint a tile with the selected material."
            };
            return iconContent;
        }
    }

    // Brush properties exposed in the tool settings
    [SerializeField]
    private Material brushMaterial; // The material to paint with

    [SerializeField, Range(0.1f, 5f)]
    private float brushSize = 1f; // Brush size (for visual preview)

    /// <summary>
    /// Draws the brush preview and handles mouse events in the Scene view.
    /// </summary>
    /// <param name="window">The active Editor window (typically a SceneView).</param>
    public override void OnToolGUI(EditorWindow window)
    {
        // Ensure we are working in the Scene view.
        SceneView sceneView = window as SceneView;
        if (sceneView == null)
            return;

        Event currentEvent = Event.current;
        // Convert the current mouse position to a world ray.
        Ray ray = HandleUtility.GUIPointToWorldRay(currentEvent.mousePosition);

        // Perform a raycast using the current physics settings.
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            // Draw a wire disc at the hit position to visualize the brush.
            Handles.color = Color.green;
            Handles.DrawWireDisc(hit.point, hit.normal, brushSize);

            // If left mouse button is pressed (and not holding Alt which is used for Scene view navigation)
            if (currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && !currentEvent.alt)
            {
                // Get the GameObject hit by the raycast.
                GameObject hitObject = hit.collider.gameObject;
                GameObject tile = hitObject;

                // If the object hit is not the actual tile (by name), traverse up the hierarchy.
                while (tile != null && !tile.name.StartsWith("Tile|"))
                {
                    if (tile.transform.parent != null)
                        tile = tile.transform.parent.gameObject;
                    else
                        break;
                }

                if (tile != null)
                {
                    // Look for the Renderer in the tile's children.
                    Renderer renderer = tile.GetComponentInChildren<Renderer>();
                    if (renderer != null)
                    {
                        // Record the change for Undo.
                        Undo.RecordObject(renderer, "Paint Tile");
                        // Apply the brush material.
                        renderer.sharedMaterial = brushMaterial;
                    }
                    else
                    {
                        Debug.LogWarning("No Renderer found in tile: " + tile.name);
                    }
                }
                // Mark the event as used to prevent further propagation.
                currentEvent.Use();
            }
        }

        // Repaint the Scene view for a smooth visual preview.
        sceneView.Repaint();
    }
}
