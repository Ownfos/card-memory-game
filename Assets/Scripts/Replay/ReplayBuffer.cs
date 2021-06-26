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
    private int nextInputEvent = 0;

    // Check if next input event should happen now.
    // This does not consume the event
    // (i.e., it will give same result until we call
    // GetClickScreenPosition to pop the actual event)
    public bool ClickHappened()
    {
        if (nextInputEvent < clickEvents.Count)
        {
            var expectedEventTime = clickEvents[nextInputEvent].time + inputSimulationStartTime;
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
        return clickEvents[nextInputEvent++].screenPosition;
    }

    public void StartSimulatingInput()
    {
        inputSimulationStartTime = Time.time;
        nextInputEvent = 0;
    }

    public void RecordStageConfiguration(List<CardType> configuration)
    {
        stageConfigurations.Add(new FixedConfiguration(configuration));
    }

    public void RecordClickEvent(ClickEvent click)
    {
        clickEvents.Add(click);
    }

    public ICardConfiguration GetStageConfiguration(int stageIndex)
    {
        return stageConfigurations[stageIndex];
    }
}
