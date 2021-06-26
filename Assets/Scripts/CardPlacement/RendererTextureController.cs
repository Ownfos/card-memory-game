using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A component that manages image texture of renderer.
// Card component uses this class to change its frontface image.
[RequireComponent(typeof(Renderer))]
public class RendererTextureController : MonoBehaviour
{
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    public void SetTexture(Texture2D texture)
    {
        material.mainTexture = texture;
    }
}
