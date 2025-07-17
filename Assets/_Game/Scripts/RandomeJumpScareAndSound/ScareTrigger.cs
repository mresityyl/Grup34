using UnityEngine;

public class ScareTrigger : MonoBehaviour
{
    public AudioSource audioSource;
    private bool isPlayed;

    private void OnTriggerEnter(Collider other)
    {
        if (isPlayed) return;

        if(other.TryGetComponent<CharacterController>(out CharacterController characterController))
            audioSource.Play();

        isPlayed = true;
    }
}
