using UnityEngine;

public class PlayDoctorRoomSound : MonoBehaviour
{
    public GameObject CorridorObject;
    public AudioClip doctorRoomClip;
    private AudioSource audioSource;
    private bool hasPlayed = false;

    void Start()
    {

        PlayerPrefs.SetInt("DoctorRoomSound", 0);

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed && other.CompareTag("Player") && PlayerPrefs.GetInt("DoctorRoomSound") == 0)
        {
            CorridorObject.GetComponent<AudioSource>().Stop();
            audioSource.PlayOneShot(doctorRoomClip);
            hasPlayed = true;
            PlayerPrefs.SetInt("DoctorRoomSound", 1);
            SoundsPrefs.instance.Missions();
        }
    }
}
