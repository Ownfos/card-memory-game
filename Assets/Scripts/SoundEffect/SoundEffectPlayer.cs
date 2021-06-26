using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles all sound effect.
// All who wants to play a sound should visit this class.
[RequireComponent(typeof(AudioSource))]
public class SoundEffectPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    // Audio clipes for each SoundEffect enum.
    // The value of enum should match the index.
    //
    // Example)
    //  soundEffects[0] should contain card select effect
    //  because SoundEffect.CardSelect is 0.
    [SerializeField] private List<AudioClip> soundEffects;

    // Find and save AudioSource component
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Play the sound effect once
    public void Play(SoundEffect effect)
    {
        audioSource.PlayOneShot(soundEffects[(int)effect]);
    }
}
