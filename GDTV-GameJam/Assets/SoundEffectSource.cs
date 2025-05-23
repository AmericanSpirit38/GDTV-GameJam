using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectSource : MonoBehaviour
{
    public static SoundEffectSource Instance { get; private set; }
    public List<AudioClip> soundEffectClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundEffect(int index)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = soundEffectClips[index];
        audioSource.PlayOneShot(soundEffectClips[index]);
    }
}