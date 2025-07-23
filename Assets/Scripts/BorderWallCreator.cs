using UnityEngine;

public class BorderWallCreator : MonoBehaviour
{
    [Header("Ground Reference")]
    public GameObject ground; // Drag your ground GameObject here

    [Header("Wall Settings")]
    public float wallThickness = 1f;
    public float wallHeight = 10f;
    public bool makeWallsInvisible = true;

    void Start()
    {
        if (ground == null)
        {
            Debug.LogError("Please assign the ground GameObject.");
            return;
        }

        // Try to get bounds (Renderer or Collider)
        Bounds bounds;
        Renderer rend = ground.GetComponent<Renderer>();
        Collider col = ground.GetComponent<Collider>();

        if (rend != null)
        {
            bounds = rend.bounds;
        }
        else if (col != null)
        {
            bounds = col.bounds;
        }
        else
        {
            Debug.LogError("Ground must have a Renderer or Collider to determine size.");
            return;
        }

        Vector3 size = bounds.size;
        Vector3 center = bounds.center;

        float halfWidth = size.x / 2f;
        float halfLength = size.z / 2f;

        GameObject borderParent = new GameObject("Map Borders");

        // Create 4 walls: Front, Back, Left, Right
        CreateWall(new Vector3(center.x, wallHeight / 2f, center.z + halfLength + wallThickness / 2f),
                   new Vector3(size.x + wallThickness * 2f, wallHeight, wallThickness),
                   borderParent.transform); // Front

        CreateWall(new Vector3(center.x, wallHeight / 2f, center.z - halfLength - wallThickness / 2f),
                   new Vector3(size.x + wallThickness * 2f, wallHeight, wallThickness),
                   borderParent.transform); // Back

        CreateWall(new Vector3(center.x + halfWidth + wallThickness / 2f, wallHeight / 2f, center.z),
                   new Vector3(wallThickness, wallHeight, size.z + wallThickness * 2f),
                   borderParent.transform); // Right

        CreateWall(new Vector3(center.x - halfWidth - wallThickness / 2f, wallHeight / 2f, center.z),
                   new Vector3(wallThickness, wallHeight, size.z + wallThickness * 2f),
                   borderParent.transform); // Left
    }

    void CreateWall(Vector3 position, Vector3 scale, Transform parent)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.position = position;
        wall.transform.localScale = scale;
        wall.transform.parent = parent;
        wall.name = "Map Border";

        Collider wallCollider = wall.GetComponent<Collider>();
        if (wallCollider != null)
        {
            wallCollider.isTrigger = false; // solid wall
        }

        if (makeWallsInvisible)
        {
            Renderer rend = wall.GetComponent<Renderer>();
            if (rend != null)
                rend.enabled = false;
        }
    }
}
