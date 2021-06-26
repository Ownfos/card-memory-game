using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Replay manager is a component that records replay data through ReplayBuffer
// and inject stage configuration and click event simulator as IClickManager.
public class ReplayManager : MonoBehaviour
{
    // A property that indicates whether we should
    // use replay buffer or provide normal game.
    // StageManager will reference this instance.
    public bool IsReplayRunning { get; set; } = false;

    // The index of gameHistories we will use to replay.
    public int ReplayHistoryIndex { get; set; }

    // The replay data we are currently writing to.
    private ReplayBuffer replayBuffer;

    // List of all ReplayBuffers generated with their record end time.
    public List<GameHistory> GameHistories { get; private set; } = new List<GameHistory>();

    // The value of Time.time when we started recording click events.
    // ReplayBuffer will record the relative time, using this value as offset.
    //
    // This allows ReplayBuffer to simulate input in the future
    // by using different offset value (the time we start simulating clicks).
    //
    // Think of this value as the time when we enter MainGame scene.
    private float recordStartTime;

    // Add stage configuration to ReplayBuffer
    public void RecordStageConfiguration(List<CardType> configuration)
    {
        replayBuffer.RecordStageConfiguration(configuration);
    }

    // Create a new replay buffer and set recordStartTime.
    public void StartRecording()
    {
        recordStartTime = Time.time;
        replayBuffer = new ReplayBuffer();
    }

    // Add a new entry to gameHistories with current replay data
    // and score if this gameplay was not triggered by a replay button.
    public void FinishRecording()
    {
        if (!IsReplayRunning)
        {
            var score = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().Score;
            GameHistories.Add(new GameHistory(score, replayBuffer));
        }
    }

    // Add the click event with relative time from record start to ReplayBuffer
    public void RecordClickEvent(ClickEvent click)
    {
        click.time -= recordStartTime;
        replayBuffer.RecordClickEvent(click);
    }

    // Get the card configuration stored in ReplayBuffer.
    // The stageIndex corresponds to the index of a stage in StageManager.stages variable.
    public ICardConfiguration GetStageConfiguration(int stageIndex)
    {
        return GameHistories[ReplayHistoryIndex].replayBuffer.GetStageConfiguration(stageIndex);
    }

    // Return current replay buffer so that it can be used as click simulator
    public ReplayBuffer GetClickSimulator()
    {
        return GameHistories[ReplayHistoryIndex].replayBuffer;
    }
}
