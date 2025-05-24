using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<bool> levelCompletionStatus;
    public void LoadLevel(int levelIdx)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIdx);
    }
}
