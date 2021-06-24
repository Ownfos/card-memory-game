﻿using UnityEngine;

public class Card : MonoBehaviour
{
    // The material of front face quad in child object.
    // The front face object has tag "FrontFace".
    private Material frontfaceMaterial;

    // The shape of this card (e.g., CardGroup.Diamond)
    private CardGroup group;
    // The number of this card (e.g., CardNumber.Two)
    private CardNumber number;

    private bool isFlipped = false;
    [SerializeField] float flipAnimationLength;

    void Awake()
    {
        // Get the Material component from child object with tag "FrontFace"
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag.Equals("FrontFace"))
            {
                frontfaceMaterial = child.GetComponent<MeshRenderer>().material;
            }
        }
    }

    public void Flip()
    {
        isFlipped = !isFlipped;
        float targetAngle = isFlipped ? 180.0f : 0.0f;
        LeanTween.rotateY(gameObject, targetAngle, flipAnimationLength);
    }

    // Test if two cards are identical
    public bool Equals(Card other)
    {
        return group == other.group && number == other.number;
    }

    // Set its group and number, while initializing its front face to specified image
    public void Initialize(CardGroup group, CardNumber number, Texture2D frontfaceTexture)
    {
        SetCardType(group, number);
        SetFrontFaceTexture(frontfaceTexture);
    }

    private void SetCardType(CardGroup group, CardNumber number)
    {
        this.group = group;
        this.number = number;
    }

    // Change the front face image of this card
    private void SetFrontFaceTexture(Texture2D texture)
    {
        frontfaceMaterial.mainTexture = texture;
    }
}
