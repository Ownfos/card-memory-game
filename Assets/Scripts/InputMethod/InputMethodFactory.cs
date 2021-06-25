using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMethodFactory
{
    public static IClickMethod GetInputMethod()
    {
        return new ClickByMouse();
    }
}
