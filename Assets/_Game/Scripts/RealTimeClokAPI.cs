using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class RealTimeClokAPI : MonoBehaviour
{
    public TextMeshProUGUI clockText;

    string timeFormat = "HH:mm:ss";
    DateTime currentTime;
    bool isSynced = false;

    void Start()
    {
        StartCoroutine(GetTime());
    }

    IEnumerator GetTime()
    {
        string primaryURL = "https://worldtimeapi.org/api/timezone/Europe/Istanbul";
        string fallbackURL = "https://timeapi.io/api/Time/current/zone?timeZone=Europe/Istanbul";

        bool success = false;

        // Ana API
        UnityWebRequest request = UnityWebRequest.Get(primaryURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            TimeDataPrimary timeData = JsonUtility.FromJson<TimeDataPrimary>(json);

            // DÜZELTME BURADA:
            DateTimeOffset dto = DateTimeOffset.Parse(timeData.datetime);
            currentTime = dto.LocalDateTime;

            success = true;
        }
        else
        {
            Debug.LogWarning("Ana API baþarýsýz: " + request.error);

            // Fallback API
            UnityWebRequest fallbackRequest = UnityWebRequest.Get(fallbackURL);
            yield return fallbackRequest.SendWebRequest();

            if (fallbackRequest.result == UnityWebRequest.Result.Success)
            {
                string json = fallbackRequest.downloadHandler.text;
                TimeDataFallback timeData = JsonUtility.FromJson<TimeDataFallback>(json);

                currentTime = new DateTime(
                    timeData.year,
                    timeData.month,
                    timeData.day,
                    timeData.hour,
                    timeData.minute,
                    timeData.seconds
                );

                success = true;
            }
            else
            {
                Debug.LogError("Yedek API da baþarýsýz: " + fallbackRequest.error);
            }
        }

        if (!success)
        {
            Debug.LogWarning("Sunucu yok, cihaz saati kullanýlýyor.");
            currentTime = DateTime.Now;
        }

        isSynced = true;
    }
    void Update()
    {
        if (!isSynced) return;

        currentTime = currentTime.AddSeconds(Time.unscaledDeltaTime);

        clockText.text = currentTime.ToString(timeFormat);
    }
}

[System.Serializable]
public class TimeDataPrimary
{
    public string datetime;
}

[System.Serializable]
public class TimeDataFallback
{
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public int seconds;
}