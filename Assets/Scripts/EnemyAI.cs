using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float attackRange = 1f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    public int maxHealth = 100;

    private int currentHealth;
    private float lastAttackTime;
    private Rigidbody rb;
    private Animator animator;
    private bool isDead = false;
    private bool isAttacking = false;

    private float baseSpeed = 2f;
    private float baseAttackCooldown = 1.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        currentHealth = maxHealth;
        UpdateAnimatorSpeeds();
    }

    void OnEnable()
    {
        isDead = false;
        isAttacking = false;
        animator.ResetTrigger("die");
        animator.SetBool("isMoving", false);
        animator.speed = 1f;

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        GetComponent<Collider>().enabled = true;
        this.enabled = true;

        AdjustDifficulty();
    }

    void FixedUpdate()
    {
        if (player == null || isDead) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;
        float distance = dir.magnitude;

        bool shouldMove = !isAttacking && distance > attackRange && distance > 0.01f;

        if (shouldMove)
        {
            MoveTowardsPlayer(dir);
        }
        else
        {
            StopMoving();
            TryAttack();
        }

        animator.SetBool("isMoving", shouldMove);
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

        animator.SetBool("isMoving", true);
        animator.speed = Mathf.Clamp(speed / baseSpeed, 0.5f, 3f);
    }

    void StopMoving()
    {
        animator.SetBool("isMoving", false);
        animator.speed = 1f;
    }

    void TryAttack()
    {
        if (isAttacking || Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        isAttacking = true;

        animator.speed = Mathf.Clamp(baseAttackCooldown / attackCooldown, 0.5f, 3f);

        if (UnityEngine.Random.value > 0.5f)
            animator.SetTrigger("attack1");
        else
            animator.SetTrigger("attack2");

        float delay = 0.4f / animator.speed;
        StartCoroutine(DealDamageAfterDelay(delay));
        StartCoroutine(ResetAttackFlagAfterTime(attackCooldown));
    }

    IEnumerator DealDamageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (player == null || isDead) yield break;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange + 0.5f)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                int damageToTake = attackDamage;

                if (DifficultyManager.instance != null)  // Null check to avoid exception
                {
                    if (DifficultyManager.instance.currentDifficulty == DifficultyManager.Difficulty.Hard)
                    {
                        damageToTake = 20;
                    }
                    else if (DifficultyManager.instance.currentDifficulty == DifficultyManager.Difficulty.Normal)
                    {
                        damageToTake = 10;
                    }
                    else if (DifficultyManager.instance.currentDifficulty == DifficultyManager.Difficulty.Easy)
                    {
                        damageToTake = 5;
                    }
                }
                else
                {
                    UnityEngine.Debug.LogWarning("DifficultyManager instance is missing");
                }

                playerHealth.TakeDamage(damageToTake);
                UnityEngine.Debug.Log("Enemy attacked player for " + damageToTake);
            }
        }
    }

    IEnumerator ResetAttackFlagAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        isAttacking = false;
        animator.ResetTrigger("attack1");
        animator.ResetTrigger("attack2");
        animator.speed = 1f;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        int damageToTake = damage;

        if (DifficultyManager.instance != null)
        {
            if (DifficultyManager.instance.currentDifficulty == DifficultyManager.Difficulty.Hard)
            {
                damageToTake = 10;
            }
            else if (DifficultyManager.instance.currentDifficulty == DifficultyManager.Difficulty.Normal)
            {
                damageToTake = 20;
            }
            else if (DifficultyManager.instance.currentDifficulty == DifficultyManager.Difficulty.Easy)
            {
                damageToTake = 25;
            }
        }

        currentHealth -= damageToTake;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UnityEngine.Debug.Log($"{gameObject.name} took {damageToTake} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        animator.SetTrigger("die");
        animator.SetBool("isMoving", false);

        GetComponent<Collider>().enabled = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        UnityEngine.Debug.Log($"{gameObject.name} died.");

        EnemySpawner spawner = FindFirstObjectByType<EnemySpawner>();
        if (spawner != null)
        {
            spawner.EnemyDied(gameObject);
        }

        ScoreManager.instance?.AddScore(10);

        StartCoroutine(DisableAfterDelay(1.5f));
    }

    IEnumerator DisableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        this.enabled = false;

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        rb.isKinematic = true;
        gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        isAttacking = false;

        animator.ResetTrigger("die");
        animator.SetBool("isMoving", false);
        animator.speed = 1f;

        rb.isKinematic = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        GetComponent<Collider>().enabled = true;
        this.enabled = true;

        UnityEngine.Debug.Log($"{gameObject.name} health reset to {currentHealth}");

        AdjustDifficulty();
    }

    private void UpdateAnimatorSpeeds()
    {
        animator.speed = 1f;
    }

    void AdjustDifficulty()
    {
        if (DifficultyManager.instance != null)
        {
            switch (DifficultyManager.instance.currentDifficulty)
            {
                case DifficultyManager.Difficulty.Easy:
                    maxHealth = 100;
                    speed = 1.5f;
                    attackDamage = 25;
                    break;
                case DifficultyManager.Difficulty.Normal:
                    maxHealth = 100;
                    speed = 2f;
                    attackDamage = 20;
                    break;
                case DifficultyManager.Difficulty.Hard:
                    maxHealth = 100;
                    speed = 2.5f;
                    attackDamage = 10;
                    break;
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("DifficultyManager instance is missing");
        }
    }
}
