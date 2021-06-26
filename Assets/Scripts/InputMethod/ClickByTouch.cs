using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implementation of IClickMethod for touch screen
public class ClickByTouch : IClickMethod
{
    public bool ClickHappened()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    public Vector2 GetClickScreenPosition()
    {
        return Input.GetTouch(0).position;
    }
}
