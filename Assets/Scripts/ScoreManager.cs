using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    private int score = 0;
    private TextMeshProUGUI scoreText;
    private string currentSceneName;

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
        currentSceneName = scene.name;

        GameObject scoreObj = GameObject.Find("ScoreText");
        if (scoreObj != null)
        {
            scoreText = scoreObj.GetComponent<TextMeshProUGUI>();
        }
        else
        {
            scoreText = null;
        }

        // Reset score for all scenes except MainMenu
        if (scene.name != "MainMenu")
        {
            ResetScore();
        }

        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    public int GetScore()
    {
        return score;
    }

    public void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public bool CheckAndSaveBestScore()
    {
        string key = "BestScore_" + currentSceneName;
        int best = PlayerPrefs.GetInt(key, 0);
        if (score > best)
        {
            PlayerPrefs.SetInt(key, score);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }

    public int GetBestScore()
    {
        string key = "BestScore_" + currentSceneName;
        return PlayerPrefs.GetInt(key, 0);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
