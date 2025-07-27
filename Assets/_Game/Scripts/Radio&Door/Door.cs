using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour, IClickable
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float closedAngle = 0f;
    [SerializeField] private float doorSpeed = 1f;
    [SerializeField] private bool isLocked;
    private bool isOpen;

    private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;

    [Header("NavMesh")]
    [SerializeField] private NavMeshObstacle navMeshObstacle;

    [Header("Locked Shake Settings")]
    [SerializeField] private float lockedShakeAngle = 5f;
    [SerializeField] private float lockedShakeDuration = 0.1f;
    private float lockedBaseAngleY;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        SetNavMeshObstacle();

        isOpen = transform.rotation.eulerAngles.y == openAngle;
        lockedBaseAngleY = transform.localEulerAngles.y;
    }
    public RadioButtonType OnClicked()
    {
        if (isLocked)
        {
            Debug.Log("Dorr is locked");

            PlayLockedAnimation();
            audioSource.clip = audioClips[2];
            audioSource.Play();

            return RadioButtonType.isNull;
        }

        if (isOpen)
        {
            audioSource.clip = audioClips[1];
            audioSource.Play();

            CloseDoor();

            isOpen = false;
        }
        else
        {
            audioSource.clip = audioClips[0];
            audioSource.Play();

            OpenDoor();
            isOpen = true;
        }

        SetNavMeshObstacle();
        return RadioButtonType.isNull;
    }

    private void OpenDoor() => InteractDoor(openAngle);
    private void CloseDoor() => InteractDoor(closedAngle);

    private void InteractDoor(float angle) => 
        transform.DORotate(new Vector3(0f, angle, 0f), doorSpeed, RotateMode.FastBeyond360);

    private void SetNavMeshObstacle()
    {
        navMeshObstacle.carving = !isOpen;
        //navMeshObstacle.enabled = isOpen;
    }

    private void PlayLockedAnimation()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOLocalRotate(
            new Vector3(0f, lockedBaseAngleY + lockedShakeAngle, 0f),
            lockedShakeDuration,
            RotateMode.FastBeyond360
        ));

        seq.Append(transform.DOLocalRotate(
            new Vector3(0f, lockedBaseAngleY - lockedShakeAngle, 0f),
            lockedShakeDuration,
            RotateMode.FastBeyond360
        ));

        seq.Append(transform.DOLocalRotate(
            new Vector3(0f, lockedBaseAngleY, 0f),
            lockedShakeDuration,
            RotateMode.FastBeyond360
        ));
    }
}
