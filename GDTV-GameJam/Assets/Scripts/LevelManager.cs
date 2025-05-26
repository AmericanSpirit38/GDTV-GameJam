using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public List<bool> levelCompletionStatus;
    public List<GameObject> levelButtons;
    public List<GameObject> locks;
    class LevelData
    {
        public bool[] levelCompletionStatus;
    }

    public void Awake()
    {
        if (FindObjectsOfType<LevelManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public void OnEnable()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            LoadData();
            UpdateLevelButtons();
        }
    }

    public void LoadLevel(int levelIdx)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIdx);
    }

    public void SaveData()
    {
        string path = Application.persistentDataPath + "/levelData.json";
        LevelData data = new LevelData();
        data.levelCompletionStatus = levelCompletionStatus.ToArray();
        string json = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(path, json);
        Debug.Log("Data saved to " + path);
    }
    
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/levelData.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            LevelData data = JsonUtility.FromJson<LevelData>(json);
            levelCompletionStatus = new List<bool>(data.levelCompletionStatus);
            Debug.Log("Data loaded from " + path);
        }
        else
        {
            Debug.LogWarning("No save file found at " + path);
        }
    }
    
    public void UpdateLevelButtons()
    {
        levelButtons.Clear();
        locks.Clear();
        levelButtons.AddRange(GameObject.FindGameObjectsWithTag("LevelButton"));
        levelButtons.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));

        locks.AddRange(GameObject.FindGameObjectsWithTag("Lock"));
        locks.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));
        Debug.Log(levelButtons);
        Debug.Log(locks);
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i < levelCompletionStatus.Count && levelCompletionStatus[i])
            {
                levelButtons[i].SetActive(true);
                locks[i].SetActive(false);
            }
            else
            {
                levelButtons[i].SetActive(false);
                locks[i].SetActive(true);
            }
        }
    }
}
