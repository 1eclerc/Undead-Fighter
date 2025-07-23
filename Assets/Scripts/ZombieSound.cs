using UnityEngine;

public class ZombieSound : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayGroan()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
