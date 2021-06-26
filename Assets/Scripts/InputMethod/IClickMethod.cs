using UnityEngine;

// This is an interface that manages click detection method.
// CardSelector uses this interface to fetch click events.
public interface IClickMethod
{
    // Returns true if input happened
    bool ClickHappened();

    // Return the position of input in screen space
    Vector2 GetClickScreenPosition();
}
