using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script enables terminating game whenever
// we press back button (Android)
public class QuitApplication : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Make sure there is only one instance of QuitApplication
        if (FindObjectsOfType<QuitApplication>().Length > 1)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
