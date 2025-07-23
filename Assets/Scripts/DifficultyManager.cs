using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    [Header("Difficulty Settings")]
    public Difficulty startingDifficulty = Difficulty.Normal;

    public static DifficultyManager instance;
    public Difficulty currentDifficulty;

    void Awake()
    {
        // Singleton pattern implementation
        if (instance == null)
        {
            instance = this;
            currentDifficulty = startingDifficulty;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UnityEngine.Debug.Log($"DifficultyManager initialized with difficulty: {currentDifficulty}");
    }

    public void SetDifficulty(Difficulty newDifficulty)
    {
        currentDifficulty = newDifficulty;
        UnityEngine.Debug.Log($"Difficulty changed to: {currentDifficulty}");
    }

    public void SetDifficulty(int difficultyIndex)
    {
        if (difficultyIndex >= 0 && difficultyIndex < System.Enum.GetValues(typeof(Difficulty)).Length)
        {
            SetDifficulty((Difficulty)difficultyIndex);
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Invalid difficulty index: {difficultyIndex}");
        }
    }

    // Helper methods for easy access
    public bool IsEasy() => currentDifficulty == Difficulty.Easy;
    public bool IsNormal() => currentDifficulty == Difficulty.Normal;
    public bool IsHard() => currentDifficulty == Difficulty.Hard;
}
