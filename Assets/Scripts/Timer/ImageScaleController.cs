using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// A component that manages image size through RectTransform scale.
// Used to control the y-scale of fill bar of timer UI.
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class ImageScaleController : MonoBehaviour
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetImageScale(Vector3 scale)
    {
        rectTransform.localScale = scale;
    }
}