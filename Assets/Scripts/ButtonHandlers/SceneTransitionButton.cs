using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionButton : MonoBehaviour
{
    // Boolean flag that prevents scene transition
    // coroutine being called multiple times.
    private bool isClicked = false;

    // Event handler for scene transition button click
    public void OnButtonClick(string sceneName)
    {
        // If this is the initial click event
        if (!isClicked)
        {
            // Mark it as clicked
            isClicked = true;

            // Move to MainGame scene
            GameObject.FindGameObjectWithTag("SceneTransition")
                .GetComponent<SceneTransition>()
                .MoveToScene(sceneName);
        }
    }
}
