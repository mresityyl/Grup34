using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource Dream1WakeUpSound;
    private void Awake()
    {
        /*
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        */
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (PlayerPrefs.GetInt("Dream1WakeUp") == 1)
        {

            Dream1WakeUpSound?.Play();
        }
        else
        {
            return;
        }
    }

}
