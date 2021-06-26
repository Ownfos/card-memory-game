using UnityEngine;

// SceneTransitionButton is a component that provides
// button click handler which enables or disables replay.
//
// To be specific, it modifies ReplayManager.IsReplayRunning
// so that components in MainGame scene can know whether
// they should replicate last gameplay or not.
public class ReplayActivationButton : MonoBehaviour
{
    // Event handler for scene transition button click.
    // Notify replay manager that we are using/not executing replay feature from now on.
    public void OnButtonClick(int historyIndex)
    {
        var replayManager = GameObject.FindGameObjectWithTag("ReplayManager").GetComponent<ReplayManager>();
        replayManager.IsReplayRunning = historyIndex != -1;
        replayManager.ReplayHistoryIndex = historyIndex;
        Debug.Log($"Replay history index {historyIndex}");
    }
}
