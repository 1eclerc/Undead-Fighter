using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    public AudioClip[] footstepClips; // Inspector'dan atanacak sesler
    private AudioSource audioSource;
    public float stepDelay = 0.5f; // Adým süresi
    private float stepTimer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            UnityEngine.Debug.LogError("AudioSource bileþeni bulunamadý!");
        }

        if (footstepClips.Length == 0)
        {
            UnityEngine.Debug.LogWarning("Ayak sesi listesi boþ! FootstepClips'e ses eklemelisin.");
        }
    }

    void Update()
    {
        // W, A, S, D tuþlarýna basýlýysa
        bool isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isWalking)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepDelay;
            }
        }
        else
        {
            stepTimer = 0f; // durunca hemen hazýr olsun
        }
    }

    void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        int index = UnityEngine.Random.Range(0, footstepClips.Length);
        AudioClip selectedClip = footstepClips[index];

        audioSource.PlayOneShot(selectedClip);
        UnityEngine.Debug.Log("Adým sesi çalýndý: " + selectedClip.name);
    }
}
