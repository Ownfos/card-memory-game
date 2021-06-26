using System;
using UnityEngine;

// Card is a component that controls state and animation of card prefab.
public class Card : MonoBehaviour
{
    // Event that triggers whenever this card is flipped
    public event EventHandler<Card> OnFlip;

    // The group and number of this card (e.g., spade jack)
    public CardType Type { get; private set; }

    // The state of this card.
    // Note that state gets fixed once it's set to CardState.FrontFaceCorrect
    // (this is done by Stage component invoking MarkAsCorrectlyFlipped() on this instsance)
    public CardState State
    {
        get { return _state; }
        set
        {
            if (_state != CardState.FrontfaceCorrect)
            {
                _state = value;
            }
        }
    }
    private CardState _state = CardState.Backface;

    // The time it takes to complete flipping motion (in seconds)
    public float FlipAnimationLength { get; private set; } = 1.0f;

    // The way flipping motion occurs
    [SerializeField] private LeanTweenType flipAnimationType;

    // Flip the card around y-axis with animation.
    public void Flip()
    {
        // Make flipped cards move forward slightly
        StartZAxisMotion();

        // Begin rotating card
        StartFlipAnimation();

        // Trigger event if we just flipped this card from backface to frontface
        if (State == CardState.Backface)
        {
            OnFlip?.Invoke(this, this);
        }

        // Mark this card as flipping
        State = CardState.Flipping;
    }

    // Fix this card flipped
    public void MarkAsCorrectlyFlipped()
    {
        State = CardState.FrontfaceCorrect;

        // Move the card back to initial position with some delay
        StartZAxisMotion(FlipAnimationLength * 0.5f);
    }

    // Set its group and number, while initializing its front face to specified image
    public void Initialize(CardType type, Texture2D frontfaceTexture)
    {
        Type = type;
        SetFrontFaceTexture(frontfaceTexture);
    }

    // Change the front face image of this card
    private void SetFrontFaceTexture(Texture2D texture)
    {
        var frontfaceTextureController = GetComponentInChildren<MeshTextureController>();
        frontfaceTextureController.SetTexture(texture);
    }

    // Begin rotating card around y-axis towards target angle
    private void StartFlipAnimation()
    {
        // Calculate target state based on current state
        var isBackface = State == CardState.Backface;
        var nextState = isBackface ? CardState.FrontfaceIncorrect : CardState.Backface;
        var targetAngle = isBackface ? 179.9f : 0.1f;

        // State motion
        LeanTween.rotateY(gameObject, targetAngle, FlipAnimationLength)
            .setEase(flipAnimationType)
            .setOnComplete(() => State = nextState);
    }

    // Begin moving card along z-axis with specified delay before motion starts
    private void StartZAxisMotion(float delay = 0.0f)
    {
        // Calculate target state based on current state
        var isBackface = State == CardState.Backface;
        var targetZ = isBackface ? -0.5f : 0.0f;

        // Start motion
        LeanTween.moveLocalZ(gameObject, targetZ, FlipAnimationLength)
            .setDelay(delay)
            .setEase(flipAnimationType);
    }
}
