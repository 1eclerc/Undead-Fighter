using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // VideoPlayer referansý

    void Start()
    {
        // VideoPlayer'ý hazýrlýyoruz
        if (videoPlayer != null)
        {
            UnityEngine.Debug.Log("Video Player found!");  // UnityEngine.Debug kullanalým
            // VideoPlayer'ý hazýrlýyoruz (bunu baþlangýçta yapýyoruz)
            videoPlayer.Prepare();

            // Hazýr olduðunda videoyu baþlat
            videoPlayer.prepareCompleted += PlayVideo;
        }
        else
        {
            UnityEngine.Debug.LogError("Video Player is not assigned!");  // UnityEngine.Debug kullanalým
        }
    }

    // Video hazýr olduðunda oynatýlacak fonksiyon
    private void PlayVideo(VideoPlayer vp)
    {
        UnityEngine.Debug.Log("Video is prepared and playing!");  // UnityEngine.Debug kullanalým
        // Video baþlatýlýyor
        vp.Play();

        // Wait for the video to end properly
        vp.loopPointReached += EndReached;

        // If you want to make sure the entire video is played, log the length.
        UnityEngine.Debug.Log($"Video length: {vp.length} seconds.");
    }

    // Video bittiðinde çaðrýlacak fonksiyon
    void EndReached(VideoPlayer vp)
    {
        // Ensure the scene transitions only after the full video ends
        UnityEngine.Debug.Log("Video finished, transitioning to MainMenu...");
        SceneManager.LoadScene("MainMenu");  // MainMenu sahnesi adýyla deðiþtirin
    }
}
