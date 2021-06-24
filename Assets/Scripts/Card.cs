using System;
using UnityEngine;

public class Card : MonoBehaviour
{
    // Event that triggers whenever this card is flipped
    public event EventHandler<Card> OnFlip;

    // The group and number of this card (e.g., spade jack)
    public CardType Type { get; private set; }

    // This variable is true when the card's front face is visible
    public bool IsFlipped { get; private set; } = false;

    // This variable is true when flipping animation is running
    public bool IsFlipAnimRunning { get; private set; } = false;

    // This variable is set to true by Stage instance
    // when a matching pair is consecutively flipped.
    public bool IsCorrectlyFlipped { get; private set; } = false;

    // The time it takes to complete flipping motion (in seconds)
    public float FlipAnimationLength { get; private set; } = 1.5f;

    // The way flipping motion occurs
    [SerializeField] private LeanTweenType flipAnimationType;

    // Test code for flipping all cards
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!IsFlipAnimRunning)
            {
                Flip();
            }
        }
    }

    // Flip the card around y-axis with animation.
    public void Flip()
    {
        // Change state
        IsFlipped = !IsFlipped;
        IsFlipAnimRunning = true;

        // Start flipping animation
        var targetAngle = IsFlipped ? 179.9f : 0.1f;
        LeanTween.rotateY(gameObject, targetAngle, FlipAnimationLength)
            .setEase(flipAnimationType)
            .setOnComplete(() => IsFlipAnimRunning = false);

        // Trigger event
        OnFlip?.Invoke(this, this);
    }

    // Set its group and number, while initializing its front face to specified image
    public void Initialize(CardType type, Texture2D frontfaceTexture)
    {
        Type = type;
        SetFrontFaceTexture(frontfaceTexture);
    }

    // Set IsCorrectlyFlipped to true
    public void MarkAsCorrectlyFlipped()
    {
        IsCorrectlyFlipped = true;
    }

    // Change the front face image of this card
    private void SetFrontFaceTexture(Texture2D texture)
    {
        var frontfaceMaterial = FindFrontFaceMaterial();
        frontfaceMaterial.mainTexture = texture;
    }

    // Get the Material component from child object with tag "FrontFace"
    private Material FindFrontFaceMaterial()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var child = transform.GetChild(i).gameObject;
            if (child.CompareTag("FrontFace"))
            {
                return child.GetComponent<MeshRenderer>().material;
            }
        }

        return null;
    }
}
