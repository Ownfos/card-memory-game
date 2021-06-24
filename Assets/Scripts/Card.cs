using System;
using UnityEngine;

// The group and number of a card (e.g. diamond 2)
public struct CardType
{
    // The shape of this card (e.g., CardGroup.Diamond)
    public CardGroup group;
    // The number of this card (e.g., CardNumber.Two)
    public CardNumber number;

    // Check if two cards are same
    public bool Equals(CardType other)
    {
        return group == other.group && number == other.number;
    }

    public static CardType RandomType()
    {
        return new CardType
        {
            group = (CardGroup)UnityEngine.Random.Range(0, 4),
            number = (CardNumber)UnityEngine.Random.Range(0, 13)
        };
    }
}

public class Card : MonoBehaviour
{
    // Event that triggers whenever this card is flipped
    public event EventHandler OnFlip;

    // The group and number of this card (e.g., spade jack)
    public CardType Type { get; private set; }

    // This variable is true when the card's front face is visible
    public bool IsFlipped { get; private set; } = false;

    // This variable is true when flipping animation is running
    public bool IsFlipping { get; private set; } = false;

    // The material of front face quad in child object.
    // The front face object has tag "FrontFace".
    private Material frontfaceMaterial;

    // The time it takes to complete flipping motion (in seconds)
    [SerializeField] private float flipAnimationLength;

    // The way flipping motion occurs
    [SerializeField] private LeanTweenType flipAnimationType;

    void Awake()
    {
        FindFrontFaceMaterial();
    }

    // Test code for flipping all cards
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if (!IsFlipping)
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
        IsFlipping = true;

        // Start flipping animation
        float targetAngle = IsFlipped ? 179.9f : 0.1f;
        LeanTween.rotateY(gameObject, targetAngle, flipAnimationLength)
            .setEase(flipAnimationType)
            .setOnComplete(() => IsFlipping = false);

        // Trigger event
        OnFlip?.Invoke(this, EventArgs.Empty);
    }

    // Check if two cards are same
    public bool Equals(Card other)
    {
        return Type.Equals(other.Type);
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
        frontfaceMaterial.mainTexture = texture;
    }

    // Get the Material component from child object with tag "FrontFace"
    private void FindFrontFaceMaterial()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag.Equals("FrontFace"))
            {
                frontfaceMaterial = child.GetComponent<MeshRenderer>().material;
            }
        }
    }
}
