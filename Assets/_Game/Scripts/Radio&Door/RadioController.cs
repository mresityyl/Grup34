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

    // Buton referanslarý
    [SerializeField] private RadioButton playButton;
    [SerializeField] private RadioButton stopButton;

    // Durum kontrolü
    private bool isPlaying = false;


    private void Start()
    {
        EtkileþimGüncelleme(); // yatak etkileþimi

        playerInput = PlayerInput.Instance;
        playerInput.OnInteractTriggered += OnClick;

        mainCamera = Camera.main;
        audioSource = GetComponent<AudioSource>();

        screenCenter = new Vector3 (Screen.width / 2, Screen.height / 2);

        stopButton?.SetPressed(true);
    }

    private void Update()
    {
        if (isPlaying && wasPlaying && !audioSource.isPlaying)
        {
            OnAudioFinished();
            isPlaying = false;
        }

        // güncel durumunu kaydet
        wasPlaying = audioSource.isPlaying;
    }

    private void OnDisable()
    {
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
        DoctorRoomSound.GetComponent<AudioSource>().Stop();
    }

    private void StopRadio()
    {
        if (!isPlaying) return;

        isPlaying = false;

        playButton?.SetPressed(false);
        stopButton?.SetPressed(true);

        audioSource.Stop();
    }


    // Dýþarýdan ses deðiþtirmek için
    public void SetAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;

        // Yeni ses geldiðinde eðer oynuyorsa durdur
        if (isPlaying)
        {
            StopRadio();
        }

        PlayRadio();
    }


    private void OnAudioFinished()
    {
        PlayerPrefs.SetInt("YatakEtkileþim", 1);
        PlayerPrefs.Save();
        SoundsPrefs.instance.Missions();
        EtkileþimGüncelleme();
    }

    private void EtkileþimGüncelleme()
    {
        if (PlayerPrefs.GetInt("YatakEtkileþim") == 1)
        {
            Bed.tag = "Yatak";
            Bed.layer = 10;
        }
    }

}

