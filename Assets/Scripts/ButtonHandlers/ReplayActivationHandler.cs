using UnityEngine;

// ReplayActivationHandler is a component that provides
// button click handler which enables or disables replay.
//
// Passing -1 as parameter means we will not use replay data.
//
// Passing non-negative integer means we will be using
// replayManager.GameHistories[historyIndex] as replay data.
public class ReplayActivationHandler : MonoBehaviour
{
    // Event handler for scene transition button click.
    // Notify replay manager that we are using/not executing replay feature from now on.
    public void OnButtonClick(int historyIndex)
    {
        var replayManager = GameObject.FindGameObjectWithTag("ReplayManager").GetComponent<ReplayManager>();
        replayManager.IsReplayRunning = historyIndex != -1;
        replayManager.ReplayHistoryIndex = historyIndex;
    }
}
