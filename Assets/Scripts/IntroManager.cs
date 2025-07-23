using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // VideoPlayer referans�

    void Start()
    {
        // VideoPlayer'� haz�rl�yoruz
        if (videoPlayer != null)
        {
            UnityEngine.Debug.Log("Video Player found!");  // UnityEngine.Debug kullanal�m
            // VideoPlayer'� haz�rl�yoruz (bunu ba�lang��ta yap�yoruz)
            videoPlayer.Prepare();

            // Haz�r oldu�unda videoyu ba�lat
            videoPlayer.prepareCompleted += PlayVideo;
        }
        else
        {
            UnityEngine.Debug.LogError("Video Player is not assigned!");  // UnityEngine.Debug kullanal�m
        }
    }

    // Video haz�r oldu�unda oynat�lacak fonksiyon
    private void PlayVideo(VideoPlayer vp)
    {
        UnityEngine.Debug.Log("Video is prepared and playing!");  // UnityEngine.Debug kullanal�m
        // Video ba�lat�l�yor
        vp.Play();

        // Wait for the video to end properly
        vp.loopPointReached += EndReached;

        // If you want to make sure the entire video is played, log the length.
        UnityEngine.Debug.Log($"Video length: {vp.length} seconds.");
    }

    // Video bitti�inde �a�r�lacak fonksiyon
    void EndReached(VideoPlayer vp)
    {
        // Ensure the scene transitions only after the full video ends
        UnityEngine.Debug.Log("Video finished, transitioning to MainMenu...");
        SceneManager.LoadScene("MainMenu");  // MainMenu sahnesi ad�yla de�i�tirin
    }
}
