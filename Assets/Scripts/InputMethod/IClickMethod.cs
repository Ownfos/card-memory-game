using UnityEngine;

// This is an interface that manages click detection method.
public interface IClickMethod
{
    // Returns true if input happened
    bool ClickHappened();

    // Return the position of input in screen space
    Vector2 GetClickScreenPosition();
}
