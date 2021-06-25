using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : MonoBehaviour
{
    // Event handler for quit button click
    public void OnButtonClick()
    {
        Application.Quit();
    }
}
