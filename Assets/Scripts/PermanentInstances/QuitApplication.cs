using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script enables terminating game whenever
// we press back button (Android)
public class QuitApplication : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
