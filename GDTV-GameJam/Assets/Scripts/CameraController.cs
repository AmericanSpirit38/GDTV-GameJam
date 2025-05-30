using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Slider soundSlider;


    private void Start()
    {
        if (soundSlider == null)
        {
            soundSlider = Array.Find(GameObject.FindObjectsOfType<Slider>(true), slider => slider.name == "Sound Slider");
        }
        soundSlider.onValueChanged.AddListener(ChangeSoundVolume);
        AudioListener.volume = 1f;
    }
    
    public void ChangeSoundVolume(float value)
    {
        AudioListener.volume = value;
        Debug.Log("Sound volume changed to: " + value);
    }
}
