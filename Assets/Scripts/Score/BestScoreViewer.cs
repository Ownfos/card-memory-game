using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
