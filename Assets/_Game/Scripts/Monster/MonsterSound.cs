using UnityEngine;

public class MonsterSound : MonoBehaviour
{
    public AudioSource source;
    public AudioClip stepSound;

    public void PlayStepSound()
    {
        source.PlayOneShot(stepSound);
    }
}
