using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Replay buffer is a class that records
// all stage configurations and click events.
// This class implements IClickMethod and can
// be used as a simulator for last gameplay.
public class ReplayBuffer : IClickMethod
{
    // List of card type configuration for each stage
    private List<FixedConfiguration> stageConfigurations = new List<FixedConfiguration>();

    // List of all click event occurances in relative time
    private List<ClickEvent> clickEvents = new List<ClickEvent>();

    // The time we started simulating click events.
    // Time stamps stored in clickEvents list will be used
    // as relative distance from this value.
    //
    // Example)
    //  inputSimulationStartTime = 2;
    //  clickEvents[0] = 3;
    //
    //  time when click event 0 should happen = 2 + 3 = 5
    private float inputSimulationStartTime;

    // The index of element in clickEvents that we should check if it occured.
    // Used while we simulate input through IClickMethod interface.
    private int nextInputEventIndex = 0;

    // Check if next input event should happen now.
    // This does not consume the event
    // (i.e., it will give same result until we call
    // GetClickScreenPosition to pop the actual event)
    public bool ClickHappened()
    {
        if (nextInputEventIndex < clickEvents.Count)
        {
            var expectedEventTime = clickEvents[nextInputEventIndex].time + inputSimulationStartTime;
            if (expectedEventTime < Time.time)
            {
                return true;
            }
        }
        return false;
    }

    // Return next click event.
    // This works like popping a queue, so next invocation will give another result.
    public Vector2 GetClickScreenPosition()
    {
        return clickEvents[nextInputEventIndex++].screenPosition;
    }

    // Reset nextInputEventIndex and record current time as start of input simulation.
    // This time value will be used as an offset for recorded input events.
    public void StartSimulatingInput()
    {
        inputSimulationStartTime = Time.time;
        nextInputEventIndex = 0;
    }

    // Add a stage configuration to the list
    public void RecordStageConfiguration(List<CardType> configuration)
    {
        stageConfigurations.Add(new FixedConfiguration(configuration));
    }

    // Add a click event to the list
    public void RecordClickEvent(ClickEvent click)
    {
        clickEvents.Add(click);
    }

    // Return a card configuration for the specified stage.
    // The stageIndex corresponds to the index of a stage in StageManager.stages variable.
    public IStageConfiguration GetStageConfiguration(int stageIndex)
    {
        return stageConfigurations[stageIndex];
    }
}
