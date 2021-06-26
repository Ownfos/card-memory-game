using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A struct that stores time and position of click event
public struct ClickEvent
{
    public ClickEvent(Vector2 screenPosition)
    {
        time = Time.time;
        this.screenPosition = screenPosition;
    }

    // Time when this event happened
    public float time;
    
    // The position of click in screen coordinate
    public Vector2 screenPosition;
}
