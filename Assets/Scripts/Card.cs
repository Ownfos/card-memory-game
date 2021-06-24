using UnityEngine;

public class Card : MonoBehaviour
{
    // The material of front face quad in child object.
    // The front face object has tag "FrontFace".
    private Material frontfaceMaterial;

    // The shape of this card (e.g., CardGroup.Diamond)
    private CardGroup group;
    // The number of this card (e.g., CardNumber.Two)
    private CardNumber number;

    // This variable is true when the card's front face is visible
    private bool isFlipped = false;

    // The time it takes to complete flipping motion (in seconds)
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

    // Flip the card around y-axis with animation.
    public void Flip()
    {
        isFlipped = !isFlipped;
        float targetAngle = isFlipped ? 179.9f : 0.1f;
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

    // Set the card group and number
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
