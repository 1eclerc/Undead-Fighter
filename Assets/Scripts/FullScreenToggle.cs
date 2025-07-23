using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggle : MonoBehaviour
{
    public Toggle fullScreenToggle;

    void Start()
    {
        // Baþlangýçta tam ekran modunu toggle durumuyla senkronize et
        fullScreenToggle.isOn = Screen.fullScreen;

        // Toggle'a bir listener ekle
        fullScreenToggle.onValueChanged.AddListener(ToggleFullScreen);
    }

    // Full screen modunu aç/kapat
    public void ToggleFullScreen(bool isFullScreen)
    {
        // Tam ekran modunu aç veya kapat
        Screen.fullScreen = isFullScreen;

        // Konsola durum yazdýr
        UnityEngine.Debug.Log("Full Screen: " + isFullScreen); // UnityEngine.Debug kullanýldý
    }
}
