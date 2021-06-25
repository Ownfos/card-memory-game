﻿using System;
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
    public float FlipAnimationLength { get; private set; } = 1.0f;

    // The way flipping motion occurs
    [SerializeField] private LeanTweenType flipAnimationType;

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

        // Make flipped cards move forward slightly
        var targetOffset = IsFlipped ? -0.5f : 0.0f;
        LeanTween.moveLocalZ(gameObject, targetOffset, FlipAnimationLength)
            .setEase(flipAnimationType);

        // Trigger event
        OnFlip?.Invoke(this, this);
    }

    // Set its group and number, while initializing its front face to specified image
    public void Initialize(CardType type, Texture2D frontfaceTexture)
    {
        Type = type;
        SetFrontFaceTexture(frontfaceTexture);
    }

    // Fix this card flipped
    public void MarkAsCorrectlyFlipped()
    {
        // Set flag to true
        IsCorrectlyFlipped = true;

        // Move the card back to where it was.
        // Note that cards move front when they are flipped.
        // This allows player to distinguish between correctly
        // flipped cards and not correctly flipped cards.
        LeanTween.moveLocalZ(gameObject, 0.0f, FlipAnimationLength)
            .setDelay(FlipAnimationLength * 0.5f)
            .setEase(flipAnimationType);
    }

    // Change the front face image of this card
    private void SetFrontFaceTexture(Texture2D texture)
    {
        var frontfaceTextureController = GetComponentInChildren<MeshTextureController>();
        frontfaceTextureController.SetTexture(texture);
    }
}
