using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    // Boolean flag that prevents scene transition
    // coroutine being called multiple times.
    private bool isStartButtonClicked = false;

    // Event handler for start button click
    public void OnStartClick()
    {
        // If this is the initial click event
        if (!isStartButtonClicked)
        {
            // Mark it as clicked
            isStartButtonClicked = true;

            // Move to MainGame scene
            GameObject.FindGameObjectWithTag("SceneTransition")
                .GetComponent<SceneTransition>()
                .MoveToScene("MainGame");
        }
    }

    // Event handler for quit button click
    public void OnQuitClick()
    {
        Application.Quit();
    }
}
