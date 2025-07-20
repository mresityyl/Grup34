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
            PlayerPrefs.SetInt("WakeUpSound", 1); // playerprefsleri sýfýrlama iþlemi için bunu silin. Daha sonra oyunu HospitalRoomMap haritasýnda oyunu bir kere baþlatýp kapatýn. Daha sonra bu satýrý geri ekleyin ve GameManager scriptindeki yeri geri yorum satýrýna alýn.
            SoundsPrefs.instance.Missions();
        }
    }
}
