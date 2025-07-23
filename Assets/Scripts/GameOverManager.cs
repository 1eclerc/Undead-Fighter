using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;

    private GameObject gameOverPanel;
    private TextMeshProUGUI finalScoreText;
    private TextMeshProUGUI bestScoreText;
    private Button retryButton;
    private Button menuButton;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        Transform panel = canvas.transform.Find("GameOverPanel");
        if (panel != null)
        {
            gameOverPanel = panel.gameObject;
            finalScoreText = panel.Find("FinalScoreText")?.GetComponent<TextMeshProUGUI>();
            bestScoreText = panel.Find("BestScoreText")?.GetComponent<TextMeshProUGUI>();
            retryButton = panel.Find("RetryButton")?.GetComponent<Button>();
            menuButton = panel.Find("MainMenuButton")?.GetComponent<Button>();

            if (retryButton != null)
            {
                retryButton.onClick.RemoveAllListeners();
                retryButton.onClick.AddListener(RetryGame);
            }

            if (menuButton != null)
            {
                menuButton.onClick.RemoveAllListeners();
                menuButton.onClick.AddListener(BackToMainMenu);
            }

            gameOverPanel.SetActive(false);
        }
        else
        {
            gameOverPanel = null;
            finalScoreText = null;
            bestScoreText = null;
        }
    }

    /// <summary>
    /// Triggers game over AFTER a delay (e.g., to allow death animation to finish).
    /// </summary>
    public void TriggerGameOverDelayed(float delay = 2f)
    {
        StartCoroutine(HandleGameOver(delay));
    }

    private IEnumerator HandleGameOver(float delay)
    {
        // Stop all enemies
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            MonoBehaviour[] scripts = enemy.GetComponents<MonoBehaviour>();
            foreach (var script in scripts)
            {
                script.enabled = false;
            }

            Animator anim = enemy.GetComponentInChildren<Animator>();
            if (anim != null) anim.SetBool("IsMoving", false);
        }

        // Stop boss
        var boss = GameObject.FindGameObjectWithTag("Boss");
        if (boss != null)
        {
            MonoBehaviour[] bossScripts = boss.GetComponents<MonoBehaviour>();
            foreach (var script in bossScripts)
            {
                script.enabled = false;
            }

            Animator anim = boss.GetComponentInChildren<Animator>();
            if (anim != null) anim.SetBool("IsMoving", false);
        }

        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            int score = ScoreManager.instance != null ? ScoreManager.instance.GetScore() : 0;
            if (finalScoreText != null)
                finalScoreText.text = "Score: " + score;

            bool newBest = ScoreManager.instance != null && ScoreManager.instance.CheckAndSaveBestScore();
            int best = ScoreManager.instance != null ? ScoreManager.instance.GetBestScore() : 0;

            if (bestScoreText != null)
            {
                if (newBest)
                    bestScoreText.text = $"<color=yellow><b>Best: {best} (New!)</b></color>";
                else
                    bestScoreText.text = $"Best: {best}";
            }
        }
        else
        {
            Debug.LogWarning("GameOverPanel not found in scene.");
        }
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        ScoreManager.instance?.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
