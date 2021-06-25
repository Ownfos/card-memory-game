using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    // Time it takes to finish fade in/out effect
    [SerializeField] private float fadeTime;

    // Easing type that controls how fading happens
    [SerializeField] private LeanTweenType fadeType;

    // The black image that covers entire screen.
    // Fade effect is implemented through modifying
    // the alpha value of this image.
    [SerializeField] private Image blackScreen;

    void Awake()
    {
        // Allow using one SceneTransition instance across all scenes
        DontDestroyOnLoad(gameObject);

        // Make sure there is only one instance of SceneTransition
        if (FindObjectsOfType<SceneTransition>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            // Being the first SceneTransition instance means
            // that the game has just began and our main screen is loaded.
            // Welcome our player with fancy black->white fade in effect!
            FadeIn();
        }
    }

    public void MoveToScene(string sceneName)
    {
        StartCoroutine(StartTransition(sceneName));
    }
    
    public void FadeOut()
    {
        LeanTween.value(gameObject, 0, 1, fadeTime).setOnUpdate((float val) =>
        {
            Color c = blackScreen.color;
            c.a = val;
            blackScreen.color = c;
        }).setEase(fadeType);
    }

    public void FadeIn()
    {
        LeanTween.value(gameObject, 1, 0, fadeTime).setOnUpdate((float val) =>
        {
            Color c = blackScreen.color;
            c.a = val;
            blackScreen.color = c;
        }).setEase(fadeType);
    }

    private IEnumerator StartTransition(string sceneName)
    {
        FadeOut();
        yield return new WaitForSeconds(fadeTime);

        SceneManager.LoadScene(sceneName);

        FadeIn();
        yield return new WaitForSeconds(fadeTime);
    }
}
