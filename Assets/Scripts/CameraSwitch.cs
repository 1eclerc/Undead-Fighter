using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;  // Main Camera referans�
    public Camera camera2;     // Camera2 referans�
    public Camera camera3;     // Camera3 referans�

    private int currentCamera = 0; // Ba�lang��ta Main Camera aktif (0: Main Camera, 1: Camera2, 2: Camera3)

    void Start()
    {
        // Ba�lang��ta sadece Main Camera aktif olacak, di�er kameralar devre d���.
        mainCamera.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
        camera3.gameObject.SetActive(false);
    }

    // Buton t�kland���nda �a�r�lacak fonksiyon
    public void SwitchCamera()
    {
        // �lk ba�ta Main Camera aktif, sonraki t�klamalarda Camera2 ve Camera3 aras�nda ge�i� yap�lacak
        if (currentCamera == 0)
        {
            // Main Camera'dan Camera2'ye ge�i�
            mainCamera.gameObject.SetActive(false);
            camera2.gameObject.SetActive(true);
            currentCamera = 1;
        }
        else if (currentCamera == 1)
        {
            // Camera2'den Camera3'e ge�i�
            camera2.gameObject.SetActive(false);
            camera3.gameObject.SetActive(true);
            currentCamera = 2;
        }
        else if (currentCamera == 2)
        {
            // Camera3'ten Main Camera'ya ge�i�
            camera3.gameObject.SetActive(false);
            mainCamera.gameObject.SetActive(true);
            currentCamera = 0;
        }
    }
}
