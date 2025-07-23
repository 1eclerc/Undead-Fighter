using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Animator animator;
    private bool isDead = false;

    public PlayerHealthUI healthUI;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponentInChildren<Animator>();
        UpdateHealthUI(); // Health UI'yý baþta güncelliyoruz
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // Zombi saldýrýsýna göre hasar ayarlarý
        int damageToTake = damage;

        if (DifficultyManager.instance != null)  // Null check for DifficultyManager.instance
        {
            if (DifficultyManager.instance.currentDifficulty == DifficultyManager.Difficulty.Hard)
            {
                damageToTake = 20;  // Hard modda zombi her saldýrdýðýnda 20 hasar alacak
            }
            else if (DifficultyManager.instance.currentDifficulty == DifficultyManager.Difficulty.Normal)
            {
                damageToTake = 10;  // Normal modda zombi her saldýrdýðýnda 10 hasar alacak
            }
            else if (DifficultyManager.instance.currentDifficulty == DifficultyManager.Difficulty.Easy)
            {
                damageToTake = 5;  // Easy modda zombi her saldýrdýðýnda 5 hasar alacak
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("DifficultyManager instance is missing");
        }

        currentHealth -= damageToTake;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Anýnda Health UI'yi güncelle
        UpdateHealthUI();
        UnityEngine.Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamageFromBullet(int damage)
    {
        // Mermi hasarý ayarlarý
        int damageToTake = damage;
        if (DifficultyManager.instance != null)  // Null check for DifficultyManager.instance
        {
            if (DifficultyManager.instance.currentDifficulty == DifficultyManager.Difficulty.Hard)
            {
                damageToTake = 10;  // Mermi hasarý Hard modda 10 olacak
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("DifficultyManager instance is missing");
        }

        currentHealth -= damageToTake;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Anýnda Health UI'yi güncelle
        UpdateHealthUI();
        UnityEngine.Debug.Log("Player Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        if (healthUI != null)
        {
            float normalized = (float)currentHealth / maxHealth;
            healthUI.SetHealth(normalized); // Health bar'ý anýnda güncelle
        }
        else
        {
            UnityEngine.Debug.LogWarning("healthUI is not assigned!");
        }
    }

    void Die()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("IsDead");
        }

        UnityEngine.Debug.Log("Player died!"); // Debug için

        // Safely disable movement and shooting
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null) movement.enabled = false;

        PlayerShoot shoot = GetComponent<PlayerShoot>();
        if (shoot != null) shoot.enabled = false;

        // Trigger Game Over panel after 2 seconds
        if (GameOverManager.instance != null)
        {
            GameOverManager.instance.TriggerGameOverDelayed(1.5f);
        }
    }

    public bool IsDead() => isDead;

    // Debug testing (press H to take damage)
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(10);  // Test for taking damage
        }
    }
}
