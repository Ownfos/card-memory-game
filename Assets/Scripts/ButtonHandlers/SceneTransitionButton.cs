using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionButton : MonoBehaviour
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
