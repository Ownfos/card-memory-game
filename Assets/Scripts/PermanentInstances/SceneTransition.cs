using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public delegate void SceneTransitionEndHandler();

    // Time it takes to finish fade in/out effect
    [SerializeField] private float fadeTime;

    // Easing type that controls how fading happens
    [SerializeField] private LeanTweenType fadeType;

    // The black image that covers entire screen.
    // Fade effect is implemented through modifying
    // the alpha value of this image.
    [SerializeField] private Image blackScreen;

    // This flag is true only when MoveToScene() function and its coroutine are running
    private bool isTransitionOngoing = false;

    void Start()
    {
        // This line is called only once when Title Screen scene is first loaded.
        // Welcome our player with fade in effect.
        FadeIn();
    }


    // Start scene transition with fade in/out effect without event handler
    public void MoveToScene(string sceneName)
    {
        StartCoroutine(StartTransition(sceneName));
    }
    
    // Gradually make the screen black
    public void FadeOut()
    {
        LeanTween.value(gameObject, 0, 1, fadeTime).setOnUpdate((float val) =>
        {
            Color c = blackScreen.color;
            c.a = val;
            blackScreen.color = c;
        }).setEase(fadeType);
    }

    // Gradually make the screen white (i.e., remove the black cover)
    public void FadeIn()
    {
        LeanTween.value(gameObject, 1, 0, fadeTime).setOnUpdate((float val) =>
        {
            Color c = blackScreen.color;
            c.a = val;
            blackScreen.color = c;
        }).setEase(fadeType);
    }

    // A coroutine that performs fade out -> load scene -> fade in consecutively
    private IEnumerator StartTransition(string sceneName)
    {
        // Ignore transition if other transition is happening
        if (isTransitionOngoing)
        {
            yield break;
        }

        // Set transition flag to true
        isTransitionOngoing = true;

        FadeOut();
        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene(sceneName);

        FadeIn();
        yield return new WaitForSeconds(fadeTime);

        // Set transition flag to false
        isTransitionOngoing = false;
    }
}
