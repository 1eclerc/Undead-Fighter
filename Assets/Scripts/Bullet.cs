using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public int damage = 25;
    public float speed = 40f;

    private void Awake()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        if (other.TryGetComponent(out EnemyAI enemy))
        {
            enemy.TakeDamage(damage);
            StopEnemyRigidbody(other);
        }
        else if (other.TryGetComponent(out BossAI boss))
        {
            boss.TakeDamage(damage);
            StopEnemyRigidbody(other);
        }

        Destroy(gameObject); // destroy regardless of what it hits
    }

    private void StopEnemyRigidbody(GameObject enemy)
    {
        if (enemy.TryGetComponent(out Rigidbody enemyRb) && !enemyRb.isKinematic)
        {
            enemyRb.linearVelocity = Vector3.zero;
            enemyRb.angularVelocity = Vector3.zero;
        }
    }
}
