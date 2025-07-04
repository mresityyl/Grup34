using System;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    [SerializeField] private AudioClip[] flashLightSFX;
     
    private Transform cameraTransform;
    private GameObject lightSource;
    private AudioSource audioSource;

    [SerializeField] private bool IsEnabled = true;
    [SerializeField] private bool IsOn;
    private readonly float slerpSpeed = 10f;

    private PlayerInput playerInput;

    private void OnDisable()
    {
        playerInput.OnFlashlightToggle -= ToggleFlashlight;
        playerInput.OnFlashlightTapFeedback -= PlaySecondaryFlashlightSound;
    }

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        lightSource = transform.GetChild(0).gameObject;
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        playerInput = PlayerInput.Instance;
        playerInput.OnFlashlightToggle += ToggleFlashlight;
        playerInput.OnFlashlightTapFeedback += PlaySecondaryFlashlightSound;

        lightSource.SetActive(false);
    }

    private void Update()
    {
        transform.SetPositionAndRotation(
            cameraTransform.position, 
            Quaternion.Slerp(transform.rotation, cameraTransform.rotation, slerpSpeed * Time.deltaTime)
        );
    }
    private void ToggleFlashlight()
    {
        if (!IsEnabled) return;

        IsOn = !IsOn;
        audioSource.PlayOneShot(flashLightSFX[0]);
    }
    private void PlaySecondaryFlashlightSound()
    {
        audioSource.PlayOneShot(flashLightSFX[1]);
        lightSource.SetActive(IsOn);
    }
}