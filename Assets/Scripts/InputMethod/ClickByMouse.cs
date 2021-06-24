using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Implementation of IClickMethod for PC
public class ClickByMouse : IClickMethod
{
    public bool ClickHappened()
    {
        return Input.GetMouseButtonDown(0);
    }

    public Vector2 GetClickScreenPosition()
    {
        return Input.mousePosition;
    }
}
