using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioSource Dream1WakeUpSound;
    private bool isGameStopped = false;
    public Volume menuBlur;
    //public Texture[] menuUi;
    //public UnityEngine.UIElements.Image image;
    public UnityEngine.UI.Image imageUI;

    public TextMeshProUGUI textUI;
     
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

    private void OnEnable()
    {
        GameInput.Instance.OnEscapeTriggered += ShowMenu;
    }

    private void OnDisable()
    {
        GameInput.Instance.OnEscapeTriggered -= ShowMenu;
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


    private void ShowMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return;

        if (!isGameStopped)
        {
            //Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //image.image = menuUi[0];
            imageUI.enabled = true;
            textUI.enabled = true;
            menuBlur.enabled = true;
            Time.timeScale = 0f;
            AudioListener.pause = true;
            isGameStopped = true;
        }
        else
        {
            Time.timeScale = 1f;

            //Cursor.lockState = CursorLockMode.None; // Kýsa süre None yap
            Cursor.lockState = CursorLockMode.Locked; // Sonra kilitle
            Cursor.visible = false;

            //Cursor.lockState = CursorLockMode.Confined;
            //image.image = null;
            imageUI.enabled = false;
            textUI.enabled = false;
            menuBlur.enabled = false;
            AudioListener.pause = false;
            isGameStopped = false;

            Application.focusChanged += OnFocusChanged;

        }
    }

    void OnFocusChanged(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Application.focusChanged -= OnFocusChanged;
        }
    }
}
