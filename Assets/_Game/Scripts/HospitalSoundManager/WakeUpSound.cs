using UnityEngine;

public class WakeUpSound : MonoBehaviour
{
    public AudioClip WakeUpClip;
    private AudioSource audioSource;
    private bool hasPlayed = false;
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        if (!hasPlayed && PlayerPrefs.GetInt("WakeUpSound") == 0)
        {
            audioSource.PlayOneShot(WakeUpClip);
            hasPlayed = true;
            PlayerPrefs.SetInt("WakeUpSound", 1); // playerprefsleri s�f�rlama i�lemi i�in bunu silin. Daha sonra oyunu HospitalRoomMap haritas�nda oyunu bir kere ba�lat�p kapat�n. Daha sonra bu sat�r� geri ekleyin ve GameManager scriptindeki yeri geri yorum sat�r�na al�n.
            SoundsPrefs.instance.Missions();
        }
    }
}
