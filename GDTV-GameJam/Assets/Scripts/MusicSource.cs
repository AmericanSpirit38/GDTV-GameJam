using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicSource : MonoBehaviour
{
    public static MusicSource Instance { get; private set; }
    public List<AudioClip> musicClips;
    [SerializeField] private Slider musicSlider;
    
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
    
    private void Start()
    {
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = musicSlider.value;
    }

    public void OnEnable()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource.clip == null && musicClips.Count > 0)
        {
            audioSource.clip = musicClips[SceneManager.GetActiveScene().buildIndex];
            audioSource.Play();
        }
    }

    public void ChangeMusicVolume(float value)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = value;
        Debug.Log("Music volume changed to: " + value);
    }
    
    public void SetMusic(int index)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicClips[index];
        audioSource.Play();
    }
}
