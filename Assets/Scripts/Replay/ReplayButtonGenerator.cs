using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This component reads all game history from ReplayManager
// and create corresponding replay buttons.
public class ReplayButtonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject replayButtonPrefab;

    void Awake()
    {
        var replayManager = GameObject.FindGameObjectWithTag("ReplayManager").GetComponent<ReplayManager>();
        if (replayManager.GameHistories.Count > 0)
        {
            DestroyInfoText();
            CreateReplayButtons(replayManager.GameHistories);
        }
    }

    // Create a button and set appripriate button click
    // handlers for each entry in gameHistories.
    private void CreateReplayButtons(List<GameHistory> gameHistories)
    {
        for (int i=0;i<gameHistories.Count; ++i)
        {
            // Instantiate replay button as child object
            var replayButton = Instantiate(replayButtonPrefab, transform);

            // Set description text
            replayButton.GetComponentInChildren<Text>().text = gameHistories[i].Description();

            // Add handler for setting replay history index for button click event
            var replayActivation = replayButton.GetComponent<ReplayActivationHandler>();
            var historyIndex = i;
            replayButton.GetComponent<Button>().onClick.AddListener(delegate { replayActivation.OnButtonClick(historyIndex); });
        }
    }

    // Destory the default info text saying "No Gameplay History"
    private void DestroyInfoText()
    {
        Destroy(GetComponentInChildren<Text>().gameObject);
    }
}
