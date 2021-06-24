using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    // The array of position where cards get instantiated
    [SerializeField] private Vector3[] cardPositions;

    // Start is called before the first frame update
    void Start()
    {
        // Get one card type for each card pair
        int numCardTypeRequired = cardPositions.Length / 2;
        HashSet<CardType> types = GenerateDifferentTypes(numCardTypeRequired);

        // Find CardGenerator component from scene
        CardGenerator generator = GameObject.FindWithTag("CardGenerator").GetComponent<CardGenerator>();

        // Create a list of random index of cardPositions variable.
        // For each type we get from "types" variable, we will pop two index
        // and assign corresponding to the newly created Card instance.
        List<int> positionIndices = GenerateRandomIndexList();

        // Instantiate a pair of card object for each type
        // and place them according to the cardPositions
        int nextPosition = 0;
        foreach(CardType type in types)
        {
            for(int i=0;i<2;++i)
            {
                GameObject card = generator.GenerateCard(type);
                card.transform.position = cardPositions[positionIndices[nextPosition++]];
            }
        }
    }

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

        while(result.Count < numTypes)
        {
            result.Add(CardType.RandomType());
        }

        return result;
    }
}
