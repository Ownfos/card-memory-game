using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// BestScoreViewer sets its Text component to show the BestScore property of ScoreManager.
[RequireComponent(typeof(Text))]
public class BestScoreViewer : MonoBehaviour
{
    void Awake()
    {
        var scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        var text = GetComponent<Text>();

        text.text = $"{scoreManager.BestScore}";
    }
}
