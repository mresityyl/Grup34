using UnityEngine;

public class PlayDoctorRoomSound : MonoBehaviour
{
    public GameObject CorridorObject, PlayButton;
    public AudioClip doctorRoomClip;
    private AudioSource audioSource;
    private bool hasPlayed = false;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("RadyoEtkileþim") == 1)
        {
            // radyo butonu ile etkileþimi açýyoruz.
            PlayButton.GetComponent<Collider>().enabled = true;
            MetallicController mc = PlayButton.GetComponent<MetallicController>();
            mc.StartMetallicEffect(2f);
        }
    }

    void Start()
    {
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
            PlayerPrefs.SetInt("RadyoEtkileþim", 1);
            PlayerPrefs.Save();
            etkileþimGüncelleme();
        }
    }


    void etkileþimGüncelleme()
    {
        if (PlayerPrefs.GetInt("RadyoEtkileþim") == 1)
        {
            // radyo butonu ile etkileþimi açýyoruz.
            PlayButton.GetComponent<Collider>().enabled = true;
            MetallicController mc = PlayButton.GetComponent<MetallicController>();
            mc.StartMetallicEffect(2f);
        }
    }
}
