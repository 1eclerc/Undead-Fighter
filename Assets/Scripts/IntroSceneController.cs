using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // UI elemanlar�n� kontrol etmek i�in gerekli
using UnityEngine.Video; // VideoPlayer kullanmak i�in gerekli
using System.Collections;  // IEnumerator i�in gerekli

public class IntroSceneController : MonoBehaviour
{
    public Button skipButton; // Skip butonunu buraya ba�layaca��z
    public VideoPlayer videoPlayer;  // VideoPlayer'� buraya ba�layaca��z
    public string mainMenuSceneName = "MainMenu";  // Main menu sahnesinin ad�
    private bool videoSkipped = false;

    void Start()
    {
        // Skip butonuna t�klanma olay�n� dinleyelim
        skipButton.onClick.AddListener(SkipIntro);

        // VideoPlayer haz�rsa, videoyu ba�lat
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += VideoFinished; // Videonun sonuna gelindi�inde �a�r�lacak
            videoPlayer.Play(); // Videoyu ba�lat
        }
        else
        {
            UnityEngine.Debug.LogError("VideoPlayer is not assigned!");  // UnityEngine.Debug kullanal�m
        }
    }

    // Videoyu bitirdi�inde bu fonksiyon �a�r�lacak
    private void VideoFinished(VideoPlayer vp)
    {
        // E�er video atlanmad�ysa, Main Menu sahnesine ge�i� yap�lacak
        if (!videoSkipped)
        {
            LoadMainMenu();
        }
    }

    // Skip butonuna t�klan�rsa bu fonksiyon �a�r�lacak
    private void SkipIntro()
    {
        videoSkipped = true;
        videoPlayer.Stop();  // Videoyu durdur
        LoadMainMenu();  // Main Menu'ye ge�i�
    }

    // Main Menu sahnesine ge�i� fonksiyonu
    private void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
