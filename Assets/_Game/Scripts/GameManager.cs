using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource Dream1WakeUpSound;
    private void Awake()
    {
        #region PlayerPrefs kay�tlar�n� s�f�rlamak i�in buray� yorum sat�r�ndan ��kar�n
        /*
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        */

        // bunu yorum sat�r�ndan ��kar�p, WakeUpSound scriptine gidin.
        #endregion
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
