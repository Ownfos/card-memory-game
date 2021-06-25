using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMethodFactory
{
    public static IClickMethod GetInputMethod()
    {
        //return new ClickByTouch();
        return new ClickByMouse();
    }
}
