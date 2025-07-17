using System;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioController : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] movementAudioClips;
    [SerializeField] private AudioClip[] heartbeatAudioClips;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource movementAudioSource;
    [SerializeField] private AudioSource heartbeatAudioSource;

    public AudioMixerGroup walkGroup;
    public AudioMixerGroup runGroup;
    public AudioMixerGroup heart;

    private Vector2 moveInput;
    private bool sprintToggledOn;
    private bool crouchToggledOn;

    [Header("Other")]
    public float sprintTimer;

    void Update()
    {
        if (moveInput.magnitude > 0.01f)
        {
            if (sprintToggledOn)
            {
                if (!crouchToggledOn)
                    TryPlaySound(movementAudioSource, movementAudioClips[1], true, true, .5f); //Run

                sprintTimer += Time.deltaTime;

                if (sprintTimer > 15)
                    TryhealtPlaySound(heartbeatAudioSource, heartbeatAudioClips[2]);
                else if (sprintTimer > 10)
                    TryhealtPlaySound(heartbeatAudioSource, heartbeatAudioClips[1]);
                else if (sprintTimer > 5)
                    TryhealtPlaySound(heartbeatAudioSource, heartbeatAudioClips[0]);
            }
            else
            {
                TryPlaySound(movementAudioSource, movementAudioClips[0], false, true, 1f); //Walk
                StopSound(heartbeatAudioSource);
                sprintTimer = 0f;
            }
        }
        else
        {
            StopSound(movementAudioSource);
            StopSound(heartbeatAudioSource);
            sprintTimer = 0f;
        }
    }

    internal void SetValues(Vector2 moveInput, bool sprintToggledOn, bool crouchToggledOn)
    {
        this.moveInput = moveInput;
        this.sprintToggledOn = sprintToggledOn;
        this.crouchToggledOn = crouchToggledOn;
    }
    private void TryPlaySound(AudioSource source, AudioClip clip, bool isRun, bool isLoop = false, float volume = 1f)
    {
        if (source.isPlaying && source.clip == clip)
            return;

        source.Stop();
        source.clip = clip;
        source.volume = volume;
        source.loop = isLoop;
        if(isRun)
            source.outputAudioMixerGroup = runGroup;
        else
            source.outputAudioMixerGroup = walkGroup;
        source.Play();
    }

    private void TryhealtPlaySound(AudioSource source, AudioClip clip, bool isLoop = false, float volume = 1f)
    {
        if (source.isPlaying && source.clip == clip)
            return;

        source.Stop();
        source.clip = clip;
        source.volume = volume;
        source.loop = isLoop;
            source.outputAudioMixerGroup = heart;
        source.Play();
    }

    private void StopSound(AudioSource source)
    {
        source.Stop();
    }
}
