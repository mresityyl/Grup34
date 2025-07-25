using UnityEngine;

public class WhisperTriggerSound : MonoBehaviour
{
    private AudioSource audioSource;
    private string soundKey = "doorSoundPlayed";  // PlayerPrefs anahtarı

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Eğer daha önce ses çalındıysa çalma
        if (!PlayerPrefs.HasKey(soundKey))
        {
            audioSource.Play();
            PlayerPrefs.SetInt(soundKey, 1);  // Çaldığını işaretle
        }
    }
}
