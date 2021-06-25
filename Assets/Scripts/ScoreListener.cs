using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This class subscribe to ScoreManager's OnScoreChange event
// to show the latest score as text UI
[RequireComponent(typeof(Text))]
public class ScoreListener : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        // Find component that displays score
        text = GetComponent<Text>();

        // Register score update handler
        var scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        scoreManager.OnScoreChange += UpdateScoreText;

    }

    private void OnDestroy()
    {
        // Unregister score update handler
        var scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        scoreManager.OnScoreChange -= UpdateScoreText;
    }

    // Change the content of Text component to latest score value
    private void UpdateScoreText(object sender, int score)
    {
        text.text = $"{score}";
    }
}
