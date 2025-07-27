using System.Collections;
using TMPro;
using UnityEngine;

public class RadioController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask layerMask;
    private Vector2 screenCenter;
    private bool wasPlaying = false;
    public GameObject Bed, DoctorRoomSound;
    public TMP_Text MissionText;

    [Header("Audio")]
    private AudioSource audioSource;

    [Header("References")]
    private PlayerInput playerInput;
    private Camera mainCamera;

    // Buton referanslar�
    [SerializeField] private RadioButton playButton;
    [SerializeField] private RadioButton stopButton;

    // Durum kontrol�
    private bool isPlaying = false;


    private void Start()
    {
        EtkilesimUpdate(); // yatak etkile�imi

        playerInput = PlayerInput.Instance;
        playerInput.OnInteractTriggered += OnClick;

        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

        //screenCenter = new Vector3 (Screen.width / 2, Screen.height / 2);

        stopButton?.SetPressed(true);
    }

    private void Update()
    {
        if (audioSource == null) return;

        if (isPlaying && wasPlaying && !audioSource.isPlaying)
        {
            OnAudioFinished();
            isPlaying = false;
        }

        // g�ncel durumunu kaydet
        wasPlaying = audioSource.isPlaying;
    }

    private void OnDisable()
    {
        if (playerInput == null) return;

        playerInput.OnInteractTriggered -= OnClick;
    }

    public void OnClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hitInfo, 3f, layerMask))
            return;


        Debug.Log(hitInfo.collider.gameObject.name);

        // Radyo butonu mu?
        if (hitInfo.collider.gameObject.TryGetComponent<IClickable>(out IClickable radioButton))
        {
            RadioButtonType radioButtonType = radioButton.OnClicked();

            switch (radioButtonType)
            {
                case RadioButtonType.Play:
                    if (!isPlaying)
                        PlayRadio();
                    break;
                case RadioButtonType.Stop:
                    if (isPlaying)
                        StopRadio();
                    break;
            }
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawRay(mainCamera.ScreenPointToRay(screenCenter));
    //}

    private void PlayRadio()
    {
        if (audioSource.clip == null) 
        {
            Debug.LogError("Audio clip is null");
            return;
        }

        isPlaying = true;

        playButton?.SetPressed(true);
        stopButton?.SetPressed(false);

        audioSource.Play();
        DoctorRoomSound?.GetComponent<AudioSource>().Stop();
    }

    private void StopRadio()
    {
        if (!isPlaying) return;

        isPlaying = false;

        playButton?.SetPressed(false);
        stopButton?.SetPressed(true);

        audioSource.Stop();
    }


    // D��ar�dan ses de�i�tirmek i�in
    public void SetAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;

        // Yeni ses geldi�inde e�er oynuyorsa durdur
        if (isPlaying)
        {
            StopRadio();
        }

        PlayRadio();
    }


    private void OnAudioFinished()
    {
        PlayerPrefs.SetInt("YatakEtkile�im", 1);
        PlayerPrefs.Save();
        SoundsPrefs.instance.Missions();
        EtkilesimUpdate();
    }

    private void EtkilesimUpdate()
    {
        if (Bed == null) return;

        if (PlayerPrefs.GetInt("YatakEtkile�im") == 1)
        {
            Bed.tag = "Yatak";
            Bed.layer = 10;
        }
    }

}

