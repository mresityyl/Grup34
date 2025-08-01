using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SoundsPrefs : MonoBehaviour
{
    public static SoundsPrefs instance;
    public TMP_Text MissionText;
    private void Awake()
    {

        instance = this;
        #region PlayerPrefs Kayıtları
        if (!PlayerPrefs.HasKey("WakeUpSound"))
        {
            PlayerPrefs.SetInt("WakeUpSound", 0);
        }
        else if (!PlayerPrefs.HasKey("CorridorSound"))
        {
            PlayerPrefs.SetInt("CorridorSound", 0);
        }
        else if (!PlayerPrefs.HasKey("DoctorRoomSound"))
        {
            PlayerPrefs.SetInt("DoctorRoomSound", 0);
        }
        else if (!PlayerPrefs.HasKey("Etkileşim"))
        {
            PlayerPrefs.SetInt("RadyoEtkileşim", 0);
        }
        else if (!PlayerPrefs.HasKey("YatakEtkileşim"))
        {
            PlayerPrefs.SetInt("YatakEtkileşim", 0);

        }
        else if (!PlayerPrefs.HasKey("Dream1WakeUp"))
        {
            PlayerPrefs.SetInt("Dream1WakeUp", 0);

        }


        #endregion
    }
    private void Start()
    {
        Missions();
    }

    public void Missions()
    {
        if(PlayerPrefs.GetInt("WakeUpSound") == 1)
        {
            MissionText.text = "Koridora Git.";
        }
        if (PlayerPrefs.GetInt("CorridorSound") == 1)
        {
            MissionText.text = "Doktorun Odasına Git.";
        }
        if (PlayerPrefs.GetInt("DoctorRoomSound") == 1)
        {
            MissionText.text = "Odana Geri Dön Ve Radyoyu Dinle.";
        }
        if (PlayerPrefs.GetInt("YatakEtkileşim") == 1)
        {
            MissionText.text = "Yatağa Yat";
        }
    }
}
