using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A component that manages image texture of mesh renderer
[RequireComponent(typeof(Renderer))]
public class MeshTextureController : MonoBehaviour
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
