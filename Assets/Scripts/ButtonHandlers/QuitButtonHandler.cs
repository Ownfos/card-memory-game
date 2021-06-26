using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Quit button simply provides a button click handler that terminates application
public class QuitButtonHandler : MonoBehaviour
{
    // Event handler for quit button click
    public void OnButtonClick()
    {
        Application.Quit();
    }
}
