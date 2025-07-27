using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScareTrigger : MonoBehaviour
{
    [Header("Ses")]
    public AudioSource audioSource;
    public AudioClip scareClip, breathClip;

    [Header("Volume ve Efektler")]
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    private LensDistortion lensDistortion;
    private DepthOfField depthOfField;

    [Header("Efekt Ayarlarý")]
    public float effectTime = 1f;
    public int loopCount = 4;

    private bool isPlayed;

    private void Start()
    {
        volume = GetComponentInParent<RandomScareManager>().volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlayed) return;

        if (!other.TryGetComponent<CharacterController>(out CharacterController _)) return;

        audioSource.volume = 0.9f;
        audioSource.PlayOneShot(scareClip);

        Player.Instance.controller.magnitude = -1f;
        Player.Instance.controller.magnitudeCamera = -1f;

        if (volume.profile.TryGet(out colorAdjustments))
        {
            DOTween.To(
                () => colorAdjustments.saturation.value,
                x => colorAdjustments.saturation.value = x,
                -100f,
                effectTime
            );
        }

        if (volume.profile.TryGet(out lensDistortion))
        {
            DOTween.To(
                () => lensDistortion.intensity.value,
                x => lensDistortion.intensity.value = x,
                -0.3f,
                effectTime
            );

            DOTween.To(
                () => lensDistortion.intensity.value,
                x => lensDistortion.intensity.value = x,
                -0.6f,
                effectTime
            )
            .SetLoops(loopCount, LoopType.Yoyo)
            .OnComplete(() =>
            {
                DOTween.To(
                    () => lensDistortion.intensity.value,
                    x => lensDistortion.intensity.value = x,
                    0f,
                    effectTime
                );

                DOTween.To(
                    () => colorAdjustments.saturation.value,
                    x => colorAdjustments.saturation.value = x,
                    0f,
                    effectTime
                );
            });
        }

        if (volume.profile.TryGet(out depthOfField))
        {
            depthOfField.active = true;

            DOTween.To(
                () => depthOfField.focusDistance.value,
                x => depthOfField.focusDistance.value = x,
                0.1f,
                effectTime
            );

            DOTween.To(
                () => depthOfField.focusDistance.value,
                x => depthOfField.focusDistance.value = x,
                0.8f,
                effectTime
            )
            .SetLoops(loopCount, LoopType.Yoyo)
            .OnComplete(() =>
            {
                DOTween.To(
                    () => depthOfField.focusDistance.value,
                    x => depthOfField.focusDistance.value = x,
                    10f,
                    effectTime
                );
                depthOfField.active = false;

                Player.Instance.controller.magnitude = 1f;
                Player.Instance.controller.magnitudeCamera = 1f;
            });

            audioSource.volume = 0.3f;
            audioSource.PlayOneShot(breathClip);
        }

        isPlayed = true;
    }
}
