using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // UI elemanlarýný kontrol etmek için gerekli
using UnityEngine.Video; // VideoPlayer kullanmak için gerekli
using System.Collections;  // IEnumerator için gerekli

public class IntroSceneController : MonoBehaviour
{
    public Button skipButton; // Skip butonunu buraya baðlayacaðýz
    public VideoPlayer videoPlayer;  // VideoPlayer'ý buraya baðlayacaðýz
    public string mainMenuSceneName = "MainMenu";  // Main menu sahnesinin adý
    private bool videoSkipped = false;

    void Start()
    {
        // Skip butonuna týklanma olayýný dinleyelim
        skipButton.onClick.AddListener(SkipIntro);

        // VideoPlayer hazýrsa, videoyu baþlat
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += VideoFinished; // Videonun sonuna gelindiðinde çaðrýlacak
            videoPlayer.Play(); // Videoyu baþlat
        }
        else
        {
            UnityEngine.Debug.LogError("VideoPlayer is not assigned!");  // UnityEngine.Debug kullanalým
        }
    }

    // Videoyu bitirdiðinde bu fonksiyon çaðrýlacak
    private void VideoFinished(VideoPlayer vp)
    {
        // Eðer video atlanmadýysa, Main Menu sahnesine geçiþ yapýlacak
        if (!videoSkipped)
        {
            LoadMainMenu();
        }
    }

    // Skip butonuna týklanýrsa bu fonksiyon çaðrýlacak
    private void SkipIntro()
    {
        videoSkipped = true;
        videoPlayer.Stop();  // Videoyu durdur
        LoadMainMenu();  // Main Menu'ye geçiþ
    }

    // Main Menu sahnesine geçiþ fonksiyonu
    private void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
