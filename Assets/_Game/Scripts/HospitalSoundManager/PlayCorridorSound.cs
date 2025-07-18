using UnityEngine;

public class PlayCorridorSound : MonoBehaviour
{
    public AudioClip corridorClip;
    private AudioSource audioSource;
    private bool hasPlayed = false;
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player"))
        {
            audioSource.PlayOneShot(corridorClip);
            hasPlayed = true;
        }
    }
}
