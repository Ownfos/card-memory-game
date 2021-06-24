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
    // The prefab of card object with front and back face image.
    [SerializeField] private GameObject cardPrefab;

    // Create a card instance and set its front face image appropriately
    public GameObject GenerateCard(CardType type)
    {
        // Get the texture to use as front face image
        Texture2D frontface = FrontFaceTexture(type);
        
        // Instantiate card object and initialize it
        GameObject card = Instantiate(cardPrefab);
        card.GetComponent<Card>().Initialize(type, frontface);

        return card;
    }

    // Return the texture for front face of card.
    //
    // Example)
    //  FrontFaceTexture(CardGroup.Spade, CardNumber.Jack) will return
    //  the card image corresponding to spade jack
    private Texture2D FrontFaceTexture(CardType type)
    {
        return cardImages[((int)type.group)].images[(int)type.number];
    }
}
