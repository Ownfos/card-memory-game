using UnityEngine;

public class ReplayActivationButton : MonoBehaviour
{
    // Event handler for scene transition button click.
    // Notify replay manager that we are using/not executing replay feature from now on.
    //
    // If parameter is false, we record the game.
    // If parameter is true, we simulate the last game with recorded gameplay data.
    public void OnButtonClick(bool active)
    {
        GameObject.FindGameObjectWithTag("ReplayManager")
            .GetComponent<ReplayManager>()
            .IsReplayRunning = active;
    }
}
