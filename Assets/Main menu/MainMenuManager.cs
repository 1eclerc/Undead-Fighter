using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;
// Removed the using System.Diagnostics line, as it's not needed

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject mapSelectPanel;
    public GameObject settingsPanel;
 

    // Settings butonuna tıklanıldığında çağrılacak fonksiyon
    public void OnSettingsButton()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnPlayButton()
    {
        mainMenuPanel.SetActive(false);
        mapSelectPanel.SetActive(true);
    }

    public void OnExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Unity Editor'ünde oyunu durdurur
#else
        Application.Quit(); // Gerçek oyunda oyun kapanır
        UnityEngine.Debug.Log("Game exited."); // Explicitly using UnityEngine.Debug
#endif
    }

    public void OnMap1Button()
    {
        UnityEngine.Debug.Log("Loading DesertScene..."); // Explicitly using UnityEngine.Debug
        SceneManager.LoadScene("DesertScene"); // Gerçek sahne adıyla değiştirin
    }

    public void OnMap2Button()
    {
        UnityEngine.Debug.Log("Loading WinterScene..."); // Explicitly using UnityEngine.Debug
        SceneManager.LoadScene("WinterScene"); // Gerçek sahne adıyla değiştirin
    }

    public void OnMap3Button()
    {
        UnityEngine.Debug.Log("Loading PineForestScene..."); // Explicitly using UnityEngine.Debug
        SceneManager.LoadScene("PineForestScene"); // Sahne adı burada "PineForestScene" olacak
    }

    public void OnBackButton()
    {
        mapSelectPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // Settings panelinden ana menüye dönmek için buton
    public void OnBackToMainMenuButton()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // Zorluk seviyesini ayarlamak için fonksiyonlar
    public void OnEasyButton()
    {
        DifficultyManager.instance.SetDifficulty(DifficultyManager.Difficulty.Easy);
    }

    public void OnNormalButton()
    {
        DifficultyManager.instance.SetDifficulty(DifficultyManager.Difficulty.Normal);
    }

    public void OnHardButton()
    {
        DifficultyManager.instance.SetDifficulty(DifficultyManager.Difficulty.Hard);
    }

    
}
