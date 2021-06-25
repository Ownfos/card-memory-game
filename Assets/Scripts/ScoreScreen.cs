using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour
{
    [SerializeField] private Text scoreText;

    void Awake()
    {
        InitializeScoreText();
    }

    // Set the content of Text component to display current score
    private void InitializeScoreText()
    {
        var scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        scoreText.text = $"Your Score: {scoreManager.Score}";
    }

    // Event handler for return button click
    public void OnReturnClick()
    {
        // Return to TitleScreen scene
        GameObject.FindGameObjectWithTag("SceneTransition")
            .GetComponent<SceneTransition>()
            .MoveToScene("TitleScreen");
    }

    // Event handler for replay button click
    public void OnReplayClick()
    {
        Debug.Log("Replay button clicked");
    }
}
