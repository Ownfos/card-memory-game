using System;
using UnityEngine;

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

    // The material of front face quad in child object.
    // The front face object has tag "FrontFace".
    private Material frontfaceMaterial;

    private CardType type;

    // This variable is true when the card's front face is visible
    private bool isFlipped = false;

    // The time it takes to complete flipping motion (in seconds)
    [SerializeField] float flipAnimationLength;

    // The way flipping motion occurs
    [SerializeField] LeanTweenType flipAnimationType;

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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Flip();
        }
    }

    // Flip the card around y-axis with animation.
    public void Flip()
    {
        // Flip card
        isFlipped = !isFlipped;
        float targetAngle = isFlipped ? 179.9f : 0.1f;
        LeanTween.rotateY(gameObject, targetAngle, flipAnimationLength).setEase(flipAnimationType);

        // Trigger event
        OnFlip?.Invoke(this, EventArgs.Empty);
    }

    // Check if two cards are same
    public bool Equals(Card other)
    {
        return type.Equals(other.type);
    }

    // Set its group and number, while initializing its front face to specified image
    public void Initialize(CardType type, Texture2D frontfaceTexture)
    {
        this.type = type;
        SetFrontFaceTexture(frontfaceTexture);
    }

    // Change the front face image of this card
    private void SetFrontFaceTexture(Texture2D texture)
    {
        frontfaceMaterial.mainTexture = texture;
    }
}
