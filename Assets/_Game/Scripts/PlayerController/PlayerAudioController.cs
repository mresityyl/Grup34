using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using static Unity.VisualScripting.Member;

public class PlayerAudioController : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip[] movementAudioClips;
    [SerializeField] private AudioClip[] heartbeatAudioClips;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource movementAudioSource;
    [SerializeField] private AudioSource heartbeatAudioSource;

    //[Header("Audio Mixer Groups")]
    //[SerializeField] private AudioMixerGroup walkGroup;
    //[SerializeField] private AudioMixerGroup runGroup;

    private Vector2 moveInput;
    private bool sprintToggledOn;
    private bool crouchToggledOn;

    [Header("Other")]
    private float sprintTimer;

    void Update()
    {
        if (moveInput.magnitude > 0.01f)
        {
            if (sprintToggledOn)
            {
                if (!crouchToggledOn)
                    TryPlaySound(movementAudioSource, movementAudioClips[1], true, .2f); //Run

                sprintTimer += Time.deltaTime;

                if (sprintTimer > 30)
                    TryHeartbeatPlaySound(heartbeatAudioSource, heartbeatAudioClips[2], .55f);
                else if (sprintTimer > 20)
                    TryHeartbeatPlaySound(heartbeatAudioSource, heartbeatAudioClips[1], .55f);
                else if (sprintTimer > 10)
                    TryHeartbeatPlaySound(heartbeatAudioSource, heartbeatAudioClips[0], .55f);
            }
            else
            {
                TryPlaySound(movementAudioSource, movementAudioClips[0], false, .45f); //Walk
                StopSound(heartbeatAudioSource, true);
                sprintTimer = 0f;
            }
        }
        else
        {
            StopSound(movementAudioSource, false);
            StopSound(heartbeatAudioSource, true);
            sprintTimer = 0f;
        }
    }

    internal void SetValues(Vector2 moveInput, bool sprintToggledOn, bool crouchToggledOn)
    {
        this.moveInput = moveInput;
        this.sprintToggledOn = sprintToggledOn;
        this.crouchToggledOn = crouchToggledOn;
    }
    private void TryPlaySound(AudioSource source, AudioClip clip, bool isRun, float volume = 1f)
    {
        if (source.isPlaying && source.clip == clip)
            return;

        source.Stop();
        source.clip = clip;
        source.volume = volume;
        source.loop = true;
        //if(isRun)
        //    source.outputAudioMixerGroup = runGroup;
        //else
        //    source.outputAudioMixerGroup = walkGroup;
        source.Play();
    }

    private void TryHeartbeatPlaySound(AudioSource source, AudioClip clip, float volume = 1f)
    {
        if (source.isPlaying && source.clip == clip)
            return;

        source.Stop();
        source.clip = clip;
        source.volume = volume;
        source.loop = false;
        source.Play();
    }
    private void StopSound(AudioSource source, bool latencySoundStop)
    {
        if(!source.isPlaying) return;

        if (latencySoundStop)
            StartCoroutine(LatencySoundStop(source));
        else
            source.Stop();
    }


    IEnumerator LatencySoundStop(AudioSource source)
    {
        TryHeartbeatPlaySound(source, heartbeatAudioClips[0], .55f);
        yield return new WaitForSeconds(4);
        source.Stop();
    }
}
