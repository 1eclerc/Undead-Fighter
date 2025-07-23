using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    [Header("Spawn Area Bounds")]
    public float minX = -10f;
    public float maxX = 10f;
    public float minZ = -10f;
    public float maxZ = 10f;

    [Header("Wave Settings")]
    public int enemiesPerWave = 3;
    public float timeBetweenWaves = 10f;
    public float spawnInterval = 1f;
    public int bossWaveInterval = 3;
    public float bossSpawnInterval = 2f;

    [Header("UI Elements")]
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI countdownText;

    [Header("Boss Scaling")]
    public int bossBaseHealth = 100;
    public int bossAttackDamage = 30;
    public float bossBaseSpeed = 1.5f;
    public float bossBaseAttackCooldown = 2f;

    [Header("Difficulty Multipliers")]
    [Header("Easy Mode")]
    public float easyZombieHealthMultiplier = 1.0f;  // Easy modunda zombi saðlýk deðeri 100 olacak
    public float easyZombieDamageMultiplier = 0.75f; // Zombi hasarýný %25 azaltýyoruz
    public float easyZombieSpeedMultiplier = 0.85f;  // Zombi hýzýný %15 azaltýyoruz
    public float easyBossHealthMultiplier = 1.0f;
    public float easyBossDamageMultiplier = 0.75f;
    public float easyBossSpeedMultiplier = 0.85f;

    [Header("Normal Mode")]
    public float normalZombieHealthMultiplier = 1.0f;
    public float normalZombieDamageMultiplier = 1.0f;
    public float normalZombieSpeedMultiplier = 1.0f;
    public float normalBossHealthMultiplier = 1.0f;
    public float normalBossDamageMultiplier = 1.0f;
    public float normalBossSpeedMultiplier = 1.0f;

    [Header("Hard Mode")]
    public float hardZombieHealthMultiplier = 1.0f;
    public float hardZombieDamageMultiplier = 1.0f;
    public float hardZombieSpeedMultiplier = 1.0f;
    public float hardBossHealthMultiplier = 1.0f;
    public float hardBossDamageMultiplier = 1.0f;
    public float hardBossSpeedMultiplier = 1.0f;

    [Header("Fallback Difficulty")]
    public DifficultyManager.Difficulty fallbackDifficulty = DifficultyManager.Difficulty.Normal;

    private int currentWave = 0;
    private float waveTimer = 0f;
    private bool spawningWave = false;

    private List<GameObject> enemyPool = new List<GameObject>();
    private int poolSize = 20;

    private List<GameObject> aliveEnemies = new List<GameObject>();

    public float spawnHeight = 50f;
    public float checkRadius = 2.5f;

    void Start()
    {
        if (enemyPrefab == null || bossPrefab == null)
        {
            UnityEngine.Debug.LogError("Enemy Prefab or Boss Prefab is not assigned.");
            return;
        }

        InitPool();
        UpdateUI();
    }

    void Update()
    {
        if (spawningWave) return;

        aliveEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);

        if (aliveEnemies.Count > 0)
        {
            countdownText.text = $"Enemies alive: {aliveEnemies.Count}";
            waveTimer = 0f;
            return;
        }

        waveTimer += Time.deltaTime;
        float timeLeft = Mathf.Max(timeBetweenWaves - waveTimer, 0);
        countdownText.text = $"Next wave in: {timeLeft:F1}s";

        if (waveTimer >= timeBetweenWaves)
        {
            StartCoroutine(SpawnWave());
            waveTimer = 0f;
        }
    }

    void InitPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);
            obj.SetActive(false);
            obj.transform.parent = this.transform;
            enemyPool.Add(obj);
        }
    }

    GameObject GetEnemyFromPool()
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)
                return enemy;
        }

        GameObject newEnemy = Instantiate(enemyPrefab);
        newEnemy.SetActive(false);
        newEnemy.transform.parent = this.transform;
        enemyPool.Add(newEnemy);
        return newEnemy;
    }

    // Helper method to get current difficulty with fallback
    private DifficultyManager.Difficulty GetCurrentDifficulty()
    {
        if (DifficultyManager.instance != null)
        {
            return DifficultyManager.instance.currentDifficulty;
        }
        else
        {
            return fallbackDifficulty;
        }
    }

    IEnumerator SpawnWave()
    {
        spawningWave = true;
        currentWave++;
        UpdateUI();

        // Get the current difficulty
        DifficultyManager.Difficulty currentDifficulty = GetCurrentDifficulty();

        // Set enemy count based on difficulty only
        int enemiesToSpawn;
        switch (currentDifficulty)
        {
            case DifficultyManager.Difficulty.Easy:
                enemiesToSpawn = 3;  // 3 zombies for Easy mode
                break;
            case DifficultyManager.Difficulty.Normal:
                enemiesToSpawn = 5;  // 5 zombies for Normal mode
                break;
            case DifficultyManager.Difficulty.Hard:
                enemiesToSpawn = 7;  // 7 zombies for Hard mode
                break;
            default:
                enemiesToSpawn = 5; // Fallback to normal
                break;
        }

        // Spawn the enemies
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }

        // Boss spawning logic
        if (bossPrefab != null && currentWave % bossWaveInterval == 0)
        {
            int bossesToSpawn = 1;

            if (currentDifficulty == DifficultyManager.Difficulty.Hard)
            {
                bossesToSpawn = 2;
            }

            for (int i = 0; i < bossesToSpawn; i++)
            {
                SpawnBoss();
                if (i < bossesToSpawn - 1)
                {
                    yield return new WaitForSeconds(bossSpawnInterval); // Boss spawn intervali burada kullanýlýyor
                }
            }
        }

        // Adjust the wave interval slightly to increase difficulty over time
        timeBetweenWaves = Mathf.Max(3f, timeBetweenWaves * 0.95f);
        spawningWave = false;
    }

    void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            UnityEngine.Debug.LogError("Enemy Prefab is not assigned.");
            return;
        }

        Vector3 spawnPos = GetSpawnPosition();
        GameObject enemy = GetEnemyFromPool();

        if (enemy != null)
        {
            enemy.SetActive(false);

            float yOffset = 1f;
            CapsuleCollider col = enemy.GetComponent<CapsuleCollider>();
            if (col != null)
            {
                yOffset = col.height / 2f;
            }

            spawnPos.y += yOffset;
            enemy.transform.position = spawnPos;
            enemy.transform.rotation = Quaternion.identity;

            EnemyAI ai = enemy.GetComponent<EnemyAI>();
            if (ai != null)
            {
                ai.enabled = true;
                ai.player = PlayerSpawner.playerTransform;
                StartCoroutine(ConfigureEnemyAfterFrame(ai));
            }

            enemy.SetActive(true);
            aliveEnemies.Add(enemy);
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null)
        {
            UnityEngine.Debug.LogError("Boss Prefab is not assigned.");
            return;
        }

        Vector3 spawnPos = GetSpawnPosition();

        CapsuleCollider bossCol = bossPrefab.GetComponent<CapsuleCollider>();
        if (bossCol != null)
        {
            spawnPos.y += bossCol.height / 2f;
        }

        GameObject boss = Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        BossAI bossAI = boss.GetComponent<BossAI>();
        if (bossAI != null)
        {
            bossAI.player = PlayerSpawner.playerTransform;

            DifficultyManager.Difficulty currentDifficulty = GetCurrentDifficulty();

            int baseHealth = bossBaseHealth + (currentWave - 1) * bossAttackDamage;
            int baseDamage = bossAttackDamage;
            float baseSpeed = bossBaseSpeed;
            float baseCooldown = bossBaseAttackCooldown;

            // Apply difficulty multipliers to boss health, damage and speed
            switch (currentDifficulty)
            {
                case DifficultyManager.Difficulty.Easy:
                    bossAI.maxHealth = Mathf.RoundToInt(baseHealth * easyBossHealthMultiplier);
                    bossAI.attackDamage = Mathf.RoundToInt(baseDamage * easyBossDamageMultiplier);
                    bossAI.speed = baseSpeed * easyBossSpeedMultiplier;
                    break;
                case DifficultyManager.Difficulty.Normal:
                    bossAI.maxHealth = Mathf.RoundToInt(baseHealth * normalBossHealthMultiplier);
                    bossAI.attackDamage = Mathf.RoundToInt(baseDamage * normalBossDamageMultiplier);
                    bossAI.speed = baseSpeed * normalBossSpeedMultiplier;
                    break;
                case DifficultyManager.Difficulty.Hard:
                    bossAI.maxHealth = Mathf.RoundToInt(baseHealth * hardBossHealthMultiplier);
                    bossAI.attackDamage = Mathf.RoundToInt(baseDamage * hardBossDamageMultiplier);
                    bossAI.speed = baseSpeed * hardBossSpeedMultiplier;
                    break;
            }

            bossAI.attackCooldown = baseCooldown;
            bossAI.ResetHealth();
            bossAI.enabled = true;
        }

        aliveEnemies.Add(boss);
    }

    Vector3 GetSpawnPosition()
    {
        const int maxAttempts = 30;

        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            float randomX = UnityEngine.Random.Range(minX, maxX);
            float randomZ = UnityEngine.Random.Range(minZ, maxZ);
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
                        if (col.gameObject.CompareTag("Obstacle"))
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

        UnityEngine.Debug.LogWarning("Could not find valid ground to spawn enemy.");
        return new Vector3(0, 1, 0);
    }

    void UpdateUI()
    {
        if (waveText != null)
            waveText.text = $"Wave: {currentWave}";
    }

    public void EnemyDied(GameObject enemy)
    {
        if (aliveEnemies.Contains(enemy))
        {
            aliveEnemies.Remove(enemy);
        }
    }

    private IEnumerator ConfigureEnemyAfterFrame(EnemyAI ai)
    {
        yield return null; // Wait one frame

        DifficultyManager.Difficulty currentDifficulty = GetCurrentDifficulty();

        float baseSpeed = 2f;
        int baseAttackDamage = 4;
        int baseHealth = ai.maxHealth > 0 ? ai.maxHealth : 100;

        switch (currentDifficulty)
        {
            case DifficultyManager.Difficulty.Easy:
                ai.speed = baseSpeed * easyZombieSpeedMultiplier;
                ai.attackDamage = Mathf.RoundToInt(baseAttackDamage * easyZombieDamageMultiplier);
                ai.maxHealth = Mathf.RoundToInt(baseHealth * easyZombieHealthMultiplier);
                break;
            case DifficultyManager.Difficulty.Normal:
                ai.speed = baseSpeed * normalZombieSpeedMultiplier;
                ai.attackDamage = Mathf.RoundToInt(baseAttackDamage * normalZombieDamageMultiplier);
                ai.maxHealth = Mathf.RoundToInt(baseHealth * normalZombieHealthMultiplier);
                break;
            case DifficultyManager.Difficulty.Hard:
                ai.speed = baseSpeed * hardZombieSpeedMultiplier;
                ai.attackDamage = Mathf.RoundToInt(baseAttackDamage * hardZombieDamageMultiplier);
                ai.maxHealth = Mathf.RoundToInt(baseHealth * hardZombieHealthMultiplier);
                break;
        }

        ai.ResetHealth();
    }
}
