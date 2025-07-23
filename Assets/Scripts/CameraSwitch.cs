using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;  // Main Camera referansý
    public Camera camera2;     // Camera2 referansý
    public Camera camera3;     // Camera3 referansý

    private int currentCamera = 0; // Baþlangýçta Main Camera aktif (0: Main Camera, 1: Camera2, 2: Camera3)

    void Start()
    {
        // Baþlangýçta sadece Main Camera aktif olacak, diðer kameralar devre dýþý.
        mainCamera.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
        camera3.gameObject.SetActive(false);
    }

    // Buton týklandýðýnda çaðrýlacak fonksiyon
    public void SwitchCamera()
    {
        // Ýlk baþta Main Camera aktif, sonraki týklamalarda Camera2 ve Camera3 arasýnda geçiþ yapýlacak
        if (currentCamera == 0)
        {
            // Main Camera'dan Camera2'ye geçiþ
            mainCamera.gameObject.SetActive(false);
            camera2.gameObject.SetActive(true);
            currentCamera = 1;
        }
        else if (currentCamera == 1)
        {
            // Camera2'den Camera3'e geçiþ
            camera2.gameObject.SetActive(false);
            camera3.gameObject.SetActive(true);
            currentCamera = 2;
        }
        else if (currentCamera == 2)
        {
            // Camera3'ten Main Camera'ya geçiþ
            camera3.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            currentCamera = 0;
        }
    }
}
