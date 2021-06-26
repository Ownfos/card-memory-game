using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SceneTransitionButtonHandler is a component that provides
// button click handler for scene transition.
public class SceneTransitionButtonHandler : MonoBehaviour
{
    // Event handler for scene transition button click
    public void OnButtonClick(string sceneName)
    {
        // Start transition to the specified scene
        GameObject.FindGameObjectWithTag("SceneTransition")
            .GetComponent<SceneTransition>()
            .MoveToScene(sceneName);
    }
}
