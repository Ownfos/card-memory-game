﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    private IClickMethod clickMethod = InputMethodFactory.GetInputMethod();
    private bool transitionHappening = false;
    private SceneTransition sceneTransition;

    private void Awake()
    {
        sceneTransition = GameObject.FindGameObjectWithTag("SceneTransition").GetComponent<SceneTransition>();
        sceneTransition.FadeIn();
    }

    // Update is called once per frame
    void Update()
    {
        if (clickMethod.ClickHappened() && !transitionHappening)
        {
            transitionHappening = true;
            sceneTransition.MoveToScene("MainGame");
        }
    }
}
