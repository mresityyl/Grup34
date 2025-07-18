using UnityEngine;

public class PlayDoctorRoomSound : MonoBehaviour
{
    public AudioClip doctorRoomClip;
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
            audioSource.PlayOneShot(doctorRoomClip);
            hasPlayed = true;
        }
    }
}
