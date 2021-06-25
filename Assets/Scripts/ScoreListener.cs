using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class subscribe to ScoreManager's OnScoreChange event
// to show the latest score as text UI
[RequireComponent(typeof(Text))]
public class ScoreListener : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("created");
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed");
    }
}
