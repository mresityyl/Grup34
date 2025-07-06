using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;

    public void Pause()
    {
        pauseMenu.SetActive(true);
    }
    public void ExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void ResumeButton()
    {
        pauseMenu.SetActive(false);
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
