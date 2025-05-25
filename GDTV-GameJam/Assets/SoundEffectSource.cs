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
        Debug.Log("Playing sound effect at index: " + index);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(soundEffectClips[index]);
        Debug.Log("Sound effect played: " + soundEffectClips[index].name);
    }
    
    public void ChangeMusicVolume(float value)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = value;
        Debug.Log("Sound Effect volume changed to: " + value);
    }
}