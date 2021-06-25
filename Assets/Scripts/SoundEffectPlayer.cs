using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles all sound effect.
// All who wants to play a sound should visit this class.
[RequireComponent(typeof(AudioSource))]
public class SoundEffectPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    // Find and save AudioSource component
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Play the sound effect once
    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
