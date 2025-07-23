using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenuManager : MonoBehaviour
{
    public Slider volumeSlider;  // Ses kontrolü için slider
    public Button easyButton;    // Easy butonu
    public Button normalButton;  // Normal butonu
    public Button hardButton;    // Hard butonu
    public Button backButton;    // Geri butonu

    private AudioManager audioManager; // AudioManager referansı

    void Start()
    {
        // Butonların Inspector'da atandığından emin ol
        if (easyButton == null || normalButton == null || hardButton == null || backButton == null || volumeSlider == null)
        {
            Debug.LogError("One or more UI components are not assigned in the Inspector!");
            return;
        }

        // DifficultyManager instance kontrolü
        if (DifficultyManager.instance == null)
        {
            Debug.LogError("DifficultyManager instance is not set! Please make sure it's in the scene.");
            return;
        }

        // AudioManager instance kontrolü
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager instance is null! Make sure AudioManager is in the scene.");
            return;
        }

        // Slider listener'larını temizle ve yeni listener ekle
        volumeSlider.onValueChanged.RemoveAllListeners();
        volumeSlider.value = audioManager.GetVolume();
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Butonlara click eventleri ekle
        easyButton.onClick.AddListener(OnEasyButton);
        normalButton.onClick.AddListener(OnNormalButton);
        hardButton.onClick.AddListener(OnHardButton);
        backButton.onClick.AddListener(OnBackToMainMenuButton);
    }

    // Ses seviyesini ayarla
    public void SetVolume(float volume)
    {
        if (audioManager != null)
        {
            audioManager.SetVolume(volume);
        }
    }

    // Zorluk seçimi fonksiyonları
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

    // Geri butonu - ana menüye dön
    public void OnBackToMainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // İstersen buraya full screen toggle fonksiyonu da ekleyebilirsin, örnek:
    /*
    public void OnFullScreenButton()
    {
        bool isFullScreen = !Screen.fullScreen;
        Screen.fullScreen = isFullScreen;
        Debug.Log("Full Screen: " + isFullScreen);
    }
    */
}
