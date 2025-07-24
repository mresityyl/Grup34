using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScareTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    private bool isPlayed;
    public Volume volume;
    private ColorAdjustments colorAdjustments;
    private LensDistortion lensDistortion;
    private DepthOfField depthOfField;

    public float effectTime = 2f;

    private void Start()
    {
        volume = GetComponentInParent<RandomScareManager>().volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isPlayed) return;

        if (!other.TryGetComponent<CharacterController>(out CharacterController characterController)) return;

        audioSource.Play();

        Player.Instance.controller.magnitude = -1f;
        Player.Instance.controller.magnitudeCamera = -1f;
        if (volume.profile.TryGet(out colorAdjustments))
        {
            //colorAdjustments.saturation.value = -100f;
            DOTween.To(
                    () => colorAdjustments.saturation.value,
                    x => colorAdjustments.saturation.value = x,
                    -100f,
                    effectTime
                );
        }
        if (volume.profile.TryGet(out lensDistortion))
        {
            //lensDistortion.intensity.value = -0.3f;
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
            ).SetLoops(4, LoopType.Yoyo)
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

            //lensDistortion.intensity.value = -0.3f;
            DOTween.To(
                  () => depthOfField.focusDistance.value,
                  x => depthOfField.focusDistance.value = x,
                  0.1f,
                  effectTime
               );

             DOTween.To(
                  () => depthOfField.focusDistance.value,
                  x => depthOfField.focusDistance.value = x,
                .8f,
                effectTime
            ).SetLoops(4, LoopType.Yoyo)
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

        }
        isPlayed = true;
    }
}