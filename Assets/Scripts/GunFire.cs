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
        if (Input.GetMouseButtonDown(0)) // sol t�k
        {
            Fire();
        }
    }

    void Fire()
    {
        UnityEngine.Debug.Log("ATE� ED�LD�");

        audioSource.PlayOneShot(audioSource.clip);
    }
}
