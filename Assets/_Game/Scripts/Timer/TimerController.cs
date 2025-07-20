using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public float totalTime = 300f; // 5 dakika = 300 saniye
    private float currentTime;

    public TMP_Text timerText; // Canvas içindeki Text komponenti referansý

    private bool isRunning = false;

    private void OnEnable()
    {
        StartTimer();
    }

    private void OnDisable()
    {
        StopTimer();
    }

    void StartTimer()
    {
        currentTime = totalTime;
        isRunning = true;
        UpdateTimerText();
    }

    void StopTimer()
    {
        isRunning = false;
    }

    void Update()
    {
        if (!isRunning)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            isRunning = false;
            TimerEnded();
        }

        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerEnded()
    {
        Debug.Log("Süre doldu!");
        SceneManager.LoadScene("HospitalRoomMap");
    }
}
