using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class VolumeControlledAudioSource : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // İlk sesi ayarla
        if (AudioManager.instance != null)
        {
            audioSource.volume = AudioManager.instance.GetVolume();
        }
    }

    void OnEnable()
    {
        AudioManager.OnVolumeChanged += UpdateVolume;
    }

    void OnDisable()
    {
        AudioManager.OnVolumeChanged -= UpdateVolume;
    }

    void UpdateVolume(float newVolume)
    {
        if (audioSource != null)
        {
            audioSource.volume = newVolume;
        }
    }
}
