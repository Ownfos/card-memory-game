using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardFrontFaceImages : MonoBehaviour
{
    // An array of card image where the index of image corresponds to its content.
    // This struct was used to facilitate 2D array configuration in Unity editor.
    //
    // Example)
    //  cardImageSet.images[0] should contain image
    //  for card 'A' because CardNumber.A is 1.
    [System.Serializable]
    struct CardImageSet
    {
        [SerializeField] public Texture2D[] images;
    }

    // An array of card image where each index corresponds to the CardGroup.
    //
    // Example)
    //  cardImages[0] should contain a CardImageSet
    //  for spade, because CardGroup.Spade is 0.
    [SerializeField] private CardImageSet[] cardImages;

    // The prefab of card object with front and back face image.
    [SerializeField] private GameObject cardPrefab;

    // Return the front face image for specified card type
    public Texture2D GetFrontFaceTexture(CardType type)
    {
        return cardImages[(int)type.group].images[(int)type.number];
    }
}
