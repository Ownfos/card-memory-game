using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// A component that manages image color
[RequireComponent(typeof(Image))]
public class ImageColorController : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetImageColor(Color color)
    {
        image.color = color;
    }
}
