using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// An array of card image where the index of image corresponds to its content
//
// Example)
//  cardImageSet.images[0] should contain image
//  for card '1' because CardNumber.One is 1.

[System.Serializable]
struct CardImageSet
{
    [SerializeField] public Texture2D[] images;
}

public class CardGenerator : MonoBehaviour
{
    // An array of card image where each index corresponds to the CardGroup.
    //
    // Example)
    //  cardImages[0] should contain a CardImageSet
    //  for spade, because CardGroup.Spade is 0.
    [SerializeField] private CardImageSet[] cardImages;
    [SerializeField] private GameObject cardPrefab;

    private Card card;

    void Start()
    {
        card = GenerateCard(RandomCardGroup(), RandomCardNumber()).GetComponent<Card>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            card.Flip();
        }
    }


    // Create a card instance and set its front face image appropriately
    public GameObject GenerateCard(CardGroup group, CardNumber number)
    {
        // Get the texture to use as front face image
        Texture2D frontface = FrontFaceTexture(group, number);
        
        // Instantiate card object and initialize it
        GameObject card = Instantiate(cardPrefab);
        card.GetComponent<Card>().Initialize(group, number, frontface);

        return card;
    }

    // Select random group from Spade, Diamond, Club, Heart.
    public static CardGroup RandomCardGroup()
    {
        return (CardGroup)Random.Range(0, 4);
    }

    // Select random number from 1 to king
    public static CardNumber RandomCardNumber()
    {
        return (CardNumber)Random.Range(0, 13);
    }

    // Return the texture for front face of card.
    //
    // Example)
    //  FrontFaceTexture(CardGroup.Spade, CardNumber.Jack) will return
    //  the card image corresponding to spade jack
    private Texture2D FrontFaceTexture(CardGroup group, CardNumber number)
    {
        return cardImages[((int)group)].images[(int)number];
    }
}
