using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject playerPrefab;

    [Header("Spawn Area Bounds")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    public float spawnHeight = 50f; // Height to raycast down from
    public float checkRadius = 1.5f; // Radius to check for obstacles near spawn point

    public static Transform playerTransform; // Static reference for enemies to use

    void Start()
    {
        Vector3 spawnPos = GetValidSpawnPosition();
        if (spawnPos != Vector3.zero)
        {
            // Get collider height (default to 2 if none found)
            float playerHeight = 2f;
            CapsuleCollider col = playerPrefab.GetComponent<CapsuleCollider>();
            if (col != null)
            {
                playerHeight = col.height;
            }

            spawnPos += Vector3.up * (playerHeight / 2f); // Offset by half collider height

            GameObject player = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            playerTransform = player.transform;

            CameraFollow.SetTarget(playerTransform);
        }
        else
        {
            Debug.LogWarning("Could not find valid ground to spawn player.");
        }
    }

    Vector3 GetValidSpawnPosition()
    {
        const int maxAttempts = 30;

        for (int i = 0; i < maxAttempts; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);
            Vector3 origin = new Vector3(randomX, spawnHeight, randomZ);

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, spawnHeight + 10f))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    Vector3 spawnPoint = hit.point;

                    Collider[] hitColliders = Physics.OverlapSphere(spawnPoint, checkRadius);
                    bool hasObstacleNearby = false;

                    foreach (var col in hitColliders)
                    {
                        if (col.CompareTag("Obstacle") || col.CompareTag("Enemy") || col.CompareTag("Player"))
                        {
                            hasObstacleNearby = true;
                            break;
                        }
                    }

                    if (!hasObstacleNearby)
                    {
                        return spawnPoint;
                    }
                }
            }
        }

        return Vector3.zero;
    }
}
