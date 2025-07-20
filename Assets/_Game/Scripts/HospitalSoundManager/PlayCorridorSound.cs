using UnityEngine;

public class PlayCorridorSound : MonoBehaviour
{
    public GameObject WakeUpObject;
    public AudioClip corridorClip;
    private AudioSource audioSource;
    private bool hasPlayed = false;
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player") && PlayerPrefs.GetInt("CorridorSound") == 0)
        {
            WakeUpObject.GetComponent<AudioSource>().Stop();
            audioSource.PlayOneShot(corridorClip);
            hasPlayed = true;
            PlayerPrefs.SetInt("CorridorSound", 1);
            SoundsPrefs.instance.Missions();
        }
    }
}
