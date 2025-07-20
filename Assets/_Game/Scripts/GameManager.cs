using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource Dream1WakeUpSound;
    private void Awake()
    {
        #region PlayerPrefs kayýtlarýný sýfýrlamak için burayý yorum satýrýndan çýkarýn
        /*
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        */

        // bunu yorum satýrýndan çýkarýp, WakeUpSound scriptine gidin.
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
