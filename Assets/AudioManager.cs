using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public float volume = 1f;
    private const string VolumePrefKey = "GameVolume";

    // Ses değişikliklerini dinlemek isteyen ses kaynakları için event
    public static event System.Action<float> OnVolumeChanged;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Kaydedilmiş sesi yükle
        volume = PlayerPrefs.GetFloat(VolumePrefKey, 1f);

        // Sahne yüklendiğinde ilk sesi gönder
        OnVolumeChanged?.Invoke(volume);
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;

        // Ayarı kaydet
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
        PlayerPrefs.Save();

        // Tüm ses kaynaklarına bildir
        OnVolumeChanged?.Invoke(volume);
    }

    public float GetVolume()
    {
        return volume;
    }
}
