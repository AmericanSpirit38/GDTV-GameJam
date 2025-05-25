using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void StartGame()
    {
        Debug.Log("Starting Game");
        SoundEffectSource.Instance.PlaySoundEffect(2);
        // Load the first level after a delay
        StartCoroutine(StartGameWithDelay(1f));
    }

    public void ExitGame()
    {
        Debug.Log("Closing Game");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else   
        Application.Quit();
#endif     
    }
    
    public IEnumerator StartGameWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(1);
    }
}
