using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    // The array of position where cards get instantiated
    [SerializeField] private Vector3[] cardPositions;

    // The Card components of each card created
    private List<Card> cards;

    // Start is called before the first frame update
    void Start()
    {
        CreateCards();
        PlaceCards();
    }

    // Change the transform of card instances in cards list
    // by assigning random element from cardPositions array as position
    private void PlaceCards()
    {
        // Create a list of random index of cardPositions variable.
        // For each type we get from "types" variable, we will pop two index
        // and assign corresponding to the newly created Card instance.
        List<int> positionIndices = GenerateRandomIndexList();

        // Place cards on the corresponding position
        int nextPosition = 0;
        foreach (Card card in cards)
        {
            card.transform.position = cardPositions[positionIndices[nextPosition++]];
        }
    }

    // Create pair of cards required to fill all cardPositions
    private void CreateCards()
    {
        // Create a list that will hold all card instances
        cards = new List<Card>();

        // Get a card type for each card pair
        HashSet<CardType> types = GenerateDifferentTypes(cardPositions.Length / 2);

        // Find CardGenerator component from scene
        CardGenerator generator = GameObject.FindWithTag("CardGenerator").GetComponent<CardGenerator>();

        // Create a pair of card for each card type
        foreach (CardType type in types)
        {
            for (int i = 0; i < 2; ++i)
            {
                // Create and register card instance
                Card card = generator.GenerateCard(type).GetComponent<Card>();
                cards.Add(card);

                // Attach flip event handler
                card.OnFlip += OnFlipHandler;
            }
        }
    }

    // Event handler for card flip event
    private void OnFlipHandler(object sender, EventArgs e)
    {
        Card card = (Card)sender;
        if (card.IsFlipped)
        {
            Debug.Log("Flip event occured");
        }
    }

    // Create a list of integers from 0 to n-1
    // where n is the length of cardPositions.
    // This indices are used to randomly place cards.
    private List<int> GenerateRandomIndexList()
    {
        // Fill the list with integers 0 ~ n-1
        List<int> result = new List<int>();
        for(int i=0;i<cardPositions.Length;++i)
        {
            result.Add(i);
        }

        // Shuffle the list
        Shuffle(result);

        return result;
    }

    // Use shuffle algorithm from stack overflow:
    // https://stackoverflow.com/questions/273313/randomize-a-listt/4262134#4262134
    private static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // Generate set of distinguishable CardType
    private HashSet<CardType> GenerateDifferentTypes(int numTypes)
    {
        HashSet<CardType> result = new HashSet<CardType>();

        while (result.Count < numTypes)
        {
            result.Add(CardType.RandomType());
        }

        return result;
    }
}
