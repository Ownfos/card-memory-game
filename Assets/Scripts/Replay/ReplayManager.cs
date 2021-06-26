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

    private ReplayBuffer replayBuffer;

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
        return replayBuffer.GetStageConfiguration(stageIndex);
    }

    // Return current replay buffer so that it can be used as click simulator
    public ReplayBuffer GetClickSimulator()
    {
        return replayBuffer;
    }
}
