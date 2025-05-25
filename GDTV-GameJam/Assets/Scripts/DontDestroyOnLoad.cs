using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    void Awake()
    {
        // Ensure this GameObject is not destroyed when loading a new scene
        DontDestroyOnLoad(gameObject);
    }
}
