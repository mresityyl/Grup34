using UnityEngine;

public class RandomScareManager : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip[] scareClips;
    public AudioSource audioSource;

    [Header("Timing Settings")]
    private float scareTimer = 0f;
    [SerializeField] private float playScareFrequency = 15f;
    [Range(1, 5)][SerializeField] private int scareChancePercent = 2;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        scareTimer += Time.deltaTime;

        if(scareTimer > playScareFrequency)
        {
            scareTimer = 0f;

            int chanceRoll = Random.Range(0, scareChancePercent);
            int clipIndex = Random.Range(0, scareClips.Length);

            if (chanceRoll == 0)
            {
                audioSource.clip = scareClips[clipIndex];
                audioSource.Play();
            }
        }
    }

}
