using UnityEngine;

public class FisiltiTriggerSound : MonoBehaviour
{
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
            Debug.Log("triggered");
            audioSource.Play();
            hasPlayed = true;
        }
    }
}
