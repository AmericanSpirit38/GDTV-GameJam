using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public void StartLevel(int levelIndex)
    {
        LevelManager.instance.LoadLevel(levelIndex);
    }
}
