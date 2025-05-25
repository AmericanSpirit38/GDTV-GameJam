using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public static CanvasController instance;
    private void Awake()
    {
        instance = this;
    }
    public Text[] texts;
    public int activeText;
    public int timeToShow;
    public float showCountdown;
    // Start is called before the first frame update
    void Start()
    {
        
   
    }

    // Update is called once per frame
    void Update()
    {
        if (showCountdown > 0)
        {
            showCountdown -= Time.deltaTime;
        }
        else
        {
            texts[activeText].enabled = false;
        }
    }
    public void showText(int textToShow)
    {
        Debug.Log(textToShow);
        foreach (var text in texts) 
        { 
            text.enabled = false;
        }
        texts[textToShow].enabled = true;
        activeText = textToShow;
        showCountdown = timeToShow;
    }
}
