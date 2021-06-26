using System;
using UnityEngine;

// Card is a component that controls state and animation of card prefab.
public class Card : MonoBehaviour
{
    // Event that triggers whenever this card is flipped from backface to frontface
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

    // Set its group and number, while initializing its frontface to the specified image
    public void Initialize(CardType type)
    {
        Type = type;

        SetFrontFaceTexture();
    }

    // Change the front face image of this card
    private void SetFrontFaceTexture()
    {
        // Find the component that gives texture for each type
        var texture = GameObject.FindGameObjectWithTag("FrontFaceImages")
            .GetComponent<CardFrontFaceImages>()
            .GetFrontFaceTexture(Type);

        // Set the texture to image that we just have found
        var frontfaceTextureController = GetComponentInChildren<MeshTextureController>();
        frontfaceTextureController.SetTexture(texture);
    }

    // Begin rotating card around y-axis towards target angle
    private void StartFlipAnimation()
    {
        // If the card is currently showing backface, we need to rotate it 180 degrees.
        // Note that target angle is neither 0 nor 180 in order to guarantee rotating direction.
        var isBackface = State == CardState.Backface;
        var targetAngle = isBackface ? 179.9f : 0.1f;

        // FrontFaceIncorrect is the default state for a card showing frontface.
        // If it was matched with a correcponding pair, Stage component will call
        // MarkAsCorrectlyFlipped() to change the state to FrontFaceCorrect.
        var nextState = isBackface ? CardState.FrontfaceIncorrect : CardState.Backface;

        // State motion
        LeanTween.rotateY(gameObject, targetAngle, FlipAnimationLength)
            .setEase(flipAnimationType)
            .setOnComplete(() => State = nextState);
    }

    // Begin moving card along z-axis with specified delay before motion starts
    private void StartZAxisMotion(float delay = 0.0f)
    {
        // If the card is currently showing backface, we need to go forward.
        // Otherwise, get back to the initial position (z = 0).
        var isBackface = State == CardState.Backface;
        var targetZ = isBackface ? -0.5f : 0.0f;

        // Start motion
        LeanTween.moveLocalZ(gameObject, targetZ, FlipAnimationLength)
            .setDelay(delay)
            .setEase(flipAnimationType);
    }
}
