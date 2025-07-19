using UnityEngine;

public class WakeUpSound : MonoBehaviour
{
    public AudioClip WakeUpClip;
    private AudioSource audioSource;
    private bool hasPlayed = false;
    void Start()
    {
        PlayerPrefs.SetInt("WakeUpSound", 0);

        audioSource = gameObject.AddComponent<AudioSource>();
        if (!hasPlayed && PlayerPrefs.GetInt("WakeUpSound") == 0)
        {
            audioSource.PlayOneShot(WakeUpClip);
            hasPlayed = true;
            PlayerPrefs.SetInt("WakeUpSound", 1);
            SoundsPrefs.instance.Missions();
        }
    }
}
