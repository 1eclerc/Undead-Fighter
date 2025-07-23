using UnityEngine;
using UnityEngine.UI;  // UnityEngine.UI.Image kulland���m�z i�in bu namespace'i ekliyoruz

public class PlayerHealthUI : MonoBehaviour
{
    public UnityEngine.UI.Image healthFillImage;  // Tam yolu kullan�yoruz

    public void SetHealth(float healthNormalized)
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = healthNormalized;
        }
    }

    // Debug Test i�in UI'n�n an�nda g�ncellenmesini sa�layan ek bir fonksiyon
    public void UpdateHealthImmediately(float healthNormalized)
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = healthNormalized;
        }
    }
}
