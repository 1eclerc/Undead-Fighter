using System.Diagnostics;
using UnityEngine;

public class GunFire : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // 2D ses
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // sol týk
        {
            Fire();
        }
    }

    void Fire()
    {
        UnityEngine.Debug.Log("ATEÞ EDÝLDÝ");

        audioSource.PlayOneShot(audioSource.clip);
    }
}
