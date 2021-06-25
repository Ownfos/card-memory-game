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

    // Index of next stage to propose
    private int nextStageIndex = 0;

    // Start main game
    void Awake()
    {
        DeactivateAllStages();
        StartFirstStage();
    }

    // Reset score and start first stage
    private void StartFirstStage()
    {
        // Reset score to zero
        var scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();
        scoreManager.ResetScore();

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

    // Load next stage or move to score room
    private void OnStageCompleteHandler(object sender, EventArgs e)
    {
        // All stages are comlete
        if (nextStageIndex >= stages.Count)
        {
            HandleGameCompletion();
        }
        // We have more stages left
        else
        {
            StartCoroutine(StartNextStage());
        }
    }

    private void HandleGameCompletion()
    {
        Debug.Log("all stages are complete!");
        GameObject.FindGameObjectWithTag("SceneTransition").GetComponent<SceneTransition>().MoveToScene("TitleScreen");
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
        var nextStage = stages[nextStageIndex++];
        nextStage.gameObject.SetActive(true);
        nextStage.configuration = new RandomConfiguration();
        nextStage.Initialize();

        // Set pop up animation for new stage
        nextStage.transform.localScale = Vector3.zero;
        LeanTween.scale(nextStage.gameObject, Vector3.one, 1.0f)
                .setEase(stageSwapEffect);

        // Attach stage completion handler
        nextStage.OnStageComplete += OnStageCompleteHandler;
    }
}
