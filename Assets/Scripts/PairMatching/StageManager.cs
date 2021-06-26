using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    // The list of stages to be proposed sequentially
    [SerializeField] private List<Stage> stages;

    // How new stages change their scale between 0 and 1
    // on stage creation and completion.
    [SerializeField] private LeanTweenType stageSwapEffect;

    // Amount of time given before timeout happens
    [SerializeField] private float timerDuration;

    // Index of next stage to propose
    private int nextStageIndex = 0;

    // Start main game
    void Awake()
    {
        DeactivateAllStages();
        StartFirstStage();
    }

    // Initialize score, timer, and first stage
    private void StartFirstStage()
    {
        // Reset score to zero
        var scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        scoreManager.ResetScore();

        // Initialize timer
        var timerManager = GameObject.FindGameObjectWithTag("TimerManager").GetComponent<TimerManager>();
        timerManager.StartTimer(timerDuration);
        timerManager.OnTimerEnd += OnTimerEndHandler;

        // Activate first stage
        StartCoroutine(StartNextStage());
    }

    private void DeactivateAllStages()
    {
        foreach(var stage in stages)
        {
            stage.gameObject.SetActive(false);
        }
    }

    // Directly end this game and move to score screen
    private void OnTimerEndHandler(object sender, EventArgs e)
    {
        StartCoroutine(HandleGameCompletion());
    }

    // Load next stage or move to score room
    private void OnStageCompleteHandler(object sender, EventArgs e)
    {
        // All stages are comlete
        if (nextStageIndex >= stages.Count)
        {
            StartCoroutine(HandleGameCompletion());
        }
        // We have more stages left
        else
        {
            StartCoroutine(StartNextStage());
        }
    }

    // After some delay, move to the ScoreScreen scene
    private IEnumerator HandleGameCompletion()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.FindGameObjectWithTag("SceneTransition")
            .GetComponent<SceneTransition>()
            .MoveToScene("ScoreScreen");
    }

    private IEnumerator StartNextStage()
    {
        // Remove current stage
        if (nextStageIndex > 0)
        {
            yield return new WaitForSeconds(0.5f);

            var currentStage = stages[nextStageIndex - 1];
            LeanTween.scale(currentStage.gameObject, Vector3.zero, 1.0f)
                .setOnComplete(() => currentStage.gameObject.SetActive(false))
                .setEase(stageSwapEffect);

            yield return new WaitForSeconds(1.0f);
        }

        // Initialize next stage
        var nextStage = stages[nextStageIndex];
        nextStage.gameObject.SetActive(true);
        nextStage.configuration = GetStageConfiguration(nextStageIndex);
        nextStage.Initialize();

        // Set pop up animation for new stage
        nextStage.transform.localScale = Vector3.zero;
        LeanTween.scale(nextStage.gameObject, Vector3.one, 1.0f)
                .setEase(stageSwapEffect);

        // Attach stage completion handler
        nextStage.OnStageComplete += OnStageCompleteHandler;

        // Increment index
        nextStageIndex++;
    }

    // If replay is enabled, return the FixedConfiguration for that stage.
    // If not, just return a RandomConfiguration instance.
    private ICardConfiguration GetStageConfiguration(int stageIndex)
    {
        var replayManager = GameObject.FindGameObjectWithTag("ReplayManager").GetComponent<ReplayManager>();
        if (replayManager.IsReplayRunning)
        {
            return replayManager.GetStageConfiguration(stageIndex);
        }

        return new RandomConfiguration();
    }
}
