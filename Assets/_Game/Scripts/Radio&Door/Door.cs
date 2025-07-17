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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        SetNavMeshObstacle();

        isOpen = transform.rotation.eulerAngles.y == openAngle;

    }
    public RadioButtonType OnClicked()
    {
        if (isLocked)
        {
            Debug.Log("Dorr is locked");
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
}
