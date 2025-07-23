using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform gunPoint;
    public float bulletLifetime = 3f;

    private Animator animator;
    private PlayerMovement playerMovement;
    private Camera mainCamera; // Cache the camera reference

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        // Cache the main camera reference
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }

        // Null kontrolü ekliyoruz.
        if (bulletPrefab == null)
        {
            UnityEngine.Debug.LogError("Bullet Prefab is not assigned.");
        }

        if (gunPoint == null)
        {
            UnityEngine.Debug.LogError("Gun Point is not assigned.");
        }

        if (playerMovement == null)
        {
            UnityEngine.Debug.LogError("Player Movement is not assigned.");
        }

        if (mainCamera == null)
        {
            UnityEngine.Debug.LogError("Main Camera not found!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Mouse sol tuşuna basıldığında
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if (animator != null)
        {
            animator.ResetTrigger("Shoot");
            animator.SetTrigger("Shoot");
        }

        // Animation ile senkronizasyon için küçük bir gecikme ekleyelim
        StartCoroutine(SpawnBulletWithDelay(0.1f));
    }

    private IEnumerator SpawnBulletWithDelay(float delay)
    {
        // Null kontrolü ekliyoruz
        if (bulletPrefab == null || gunPoint == null)
        {
            UnityEngine.Debug.LogError("Bullet Prefab or Gun Point is not assigned.");
            yield break; // Eğer null ise, coroutine'i durduruyoruz.
        }

        if (mainCamera == null)
        {
            UnityEngine.Debug.LogError("Main Camera is null, cannot shoot!");
            yield break;
        }

        yield return new WaitForSeconds(delay);

        // Mouse konumuna göre bir ray oluştur
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = Physics.Raycast(ray, out RaycastHit hit) ? hit.point : ray.GetPoint(1000);

        // Hedef noktasına yönelmek için vektör oluştur
        Vector3 direction = targetPoint - gunPoint.position;
        direction = direction.normalized;

        // Yalnızca yatayda (y ekseninde) yön değiştirerek oyuncuyu hedefe döndür
        if (playerMovement != null)
        {
            Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z);
            if (flatDirection != Vector3.zero)
            {
                playerMovement.LockRotationToShootDirection(flatDirection);
            }
        }

        // Silahı hedefe yönlendir
        gunPoint.rotation = Quaternion.LookRotation(direction);

        // Mermi oluştur
        GameObject bullet = Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            float speed = bulletScript != null ? bulletScript.speed : 40f;
            rb.linearVelocity = direction * speed;  // Tam 3D hız kullanarak mermiye yön ver

            // Oyuncu ile çarpışmayı engelle
            Collider bulletCol = bullet.GetComponent<Collider>();
            Collider playerCol = GetComponent<Collider>();
            if (bulletCol != null && playerCol != null)
            {
                Physics.IgnoreCollision(bulletCol, playerCol);
            }
        }

        Destroy(bullet, bulletLifetime);

        // Mermiyi oluşturduktan sonra, oyuncu hareket ediyorsa animasyon durumunu güncelle
        if (animator != null && playerMovement != null)
        {
            animator.SetBool("IsMoving", playerMovement.IsMoving());
        }
    }
}