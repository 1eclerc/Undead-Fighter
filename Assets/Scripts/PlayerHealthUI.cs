using UnityEngine;
using UnityEngine.UI;  // UnityEngine.UI.Image kullandýðýmýz için bu namespace'i ekliyoruz

public class PlayerHealthUI : MonoBehaviour
{
    public UnityEngine.UI.Image healthFillImage;  // Tam yolu kullanýyoruz

    public void SetHealth(float healthNormalized)
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = healthNormalized;
        }
    }

    // Debug Test için UI'nýn anýnda güncellenmesini saðlayan ek bir fonksiyon
    public void UpdateHealthImmediately(float healthNormalized)
    {
        if (healthFillImage != null)
        {
            healthFillImage.fillAmount = healthNormalized;
        }
    }
}
