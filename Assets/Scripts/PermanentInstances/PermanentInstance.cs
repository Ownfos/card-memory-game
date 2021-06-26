using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script makes the gameobject attached permanent,
// allowing it to stay active through scene transitions.
// Only one instance of PermanentInstance component is allowed.
//
// Attach this component to the parent of all objects
// that should persist throughout scene transitions.
// See TitleScreen scene for specific example.
public class PermanentInstance : MonoBehaviour
{
    void Awake()
    {
        // Maintain instance on scene transition
        DontDestroyOnLoad(gameObject);

        // Make sure there is only one instance
        if (FindObjectsOfType<PermanentInstance>().Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
