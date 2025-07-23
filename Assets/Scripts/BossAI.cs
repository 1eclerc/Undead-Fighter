using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class BossAI : MonoBehaviour
{
    public Transform player;
    public float speed = 1.5f;
    public float attackRange = 2f;
    public int attackDamage = 30;
    public float attackCooldown = 2f;
    public int maxHealth = 250;

    private int currentHealth;
    private float lastAttackTime;
    private Rigidbody rb;
    private Animator animator;
    private bool isDead = false;

    private float baseSpeed = 1.5f;
    private float baseAttackCooldown = 2f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        currentHealth = maxHealth;

        UpdateAnimatorSpeeds();
    }

    void FixedUpdate()
    {
        if (player == null || isDead) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        float distance = dir.magnitude;

        if (distance > attackRange && distance > 0.1f)
        {
            MoveTowardsPlayer(dir);
        }
        else
        {
            StopMoving();
            TryAttack();
        }
    }

    void MoveTowardsPlayer(Vector3 dir)
    {
        Vector3 move = dir.normalized * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        if (dir != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, rot, Time.fixedDeltaTime * 5f));
        }

        animator.SetBool("Move01", true);
        animator.speed = Mathf.Clamp(speed / baseSpeed, 0.5f, 3f);
    }

    void StopMoving()
    {
        animator.SetBool("Move01", false);
        animator.speed = 1f;
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            animator.speed = Mathf.Clamp(baseAttackCooldown / attackCooldown, 0.5f, 3f);

            animator.SetTrigger("Attack01");
            StartCoroutine(DealDamageAfterDelay(0.5f / animator.speed));
        }
    }

    IEnumerator DealDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (player == null || isDead) yield break;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange + 1f)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
                UnityEngine.Debug.Log("Boss attacked player for " + attackDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        UnityEngine.Debug.Log($"{gameObject.name} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;

        animator.ResetTrigger("Die01");

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        GetComponent<Collider>().enabled = true;
        this.enabled = true;

        UpdateAnimatorSpeeds();

        UnityEngine.Debug.Log($"{gameObject.name} health reset to {currentHealth}");
    }

    void Die()
    {
        isDead = true;

        animator.SetTrigger("Die01");

        rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        this.enabled = false;

        UnityEngine.Debug.Log($"{gameObject.name} died.");

        EnemySpawner spawner = FindObjectOfType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.EnemyDied(gameObject);
        }

        ScoreManager.instance?.AddScore(30);

        StartCoroutine(DisableAfterDelay(0.8f));
    }

    IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    private void UpdateAnimatorSpeeds()
    {
        animator.speed = 1f;
    }

    void AdjustDifficulty()
    {
        switch (DifficultyManager.instance.currentDifficulty)
        {
            case DifficultyManager.Difficulty.Easy:
                maxHealth = 250;
                speed = 1.5f;
                attackDamage = 30;
                break;
            case DifficultyManager.Difficulty.Normal:
                maxHealth = 300;
                speed = 2f;
                attackDamage = 40;
                break;
            case DifficultyManager.Difficulty.Hard:
                maxHealth = 500;  // Increased health for Hard mode
                speed = 3f;       // Increased speed for Hard mode
                attackDamage = 60; // Increased damage for Hard mode
                break;
        }
    }
}
