using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggle : MonoBehaviour
{
    public Toggle fullScreenToggle;

    void Start()
    {
        // Ba�lang��ta tam ekran modunu toggle durumuyla senkronize et
        fullScreenToggle.isOn = Screen.fullScreen;

        // Toggle'a bir listener ekle
        fullScreenToggle.onValueChanged.AddListener(ToggleFullScreen);
    }

    // Full screen modunu a�/kapat
    public void ToggleFullScreen(bool isFullScreen)
    {
        // Tam ekran modunu a� veya kapat
        Screen.fullScreen = isFullScreen;

        // Konsola durum yazd�r
        UnityEngine.Debug.Log("Full Screen: " + isFullScreen); // UnityEngine.Debug kullan�ld�
    }
}
