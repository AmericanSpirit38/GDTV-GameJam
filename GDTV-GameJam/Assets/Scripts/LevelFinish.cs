using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    private void OnTrriggerEnter(Collision other)
    {
        if (other.gameObject.CompareTag("LevelExit"))
        {
            FinishLevel();
        }
    }

    private static void FinishLevel()
    {
        Debug.Log("Level Finished");
        SoundEffectSource.Instance.PlaySoundEffect(3);
        if (SceneManager.GetActiveScene().buildIndex != 7) // Assuming level 7 is the last level
        {
            LevelManager.instance.levelCompletionStatus[SceneManager.GetActiveScene().buildIndex - 1] = true;
            LevelManager.instance.SaveData();
        }
        else
        {
            Debug.Log("All levels completed! Starting victory scene.");
            LevelManager.instance.levelCompletionStatus[SceneManager.GetActiveScene().buildIndex - 1] = true;
            LevelManager.instance.SaveData();
            SceneManager.LoadScene(8); // Assuming scene 8 is the victory scene
        }
        SceneManager.LoadScene(1);
    }
}
