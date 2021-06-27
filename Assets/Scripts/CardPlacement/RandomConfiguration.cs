using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// RandomPlacement is a class that dynamically generates
// a random array of CardType when requested.
public class RandomConfiguration : IStageConfiguration
{

    // Give a randomized card type configuration through following steps:
    // 1. select n/2 different CardTypes
    // 2. create a list from the selected types
    // 3. randomly shuffle the list
    public List<CardType> GetStageConfiguration(int numCards)
    {
        var numPairs = numCards / 2;
        var typePerPair = CreateRandomTypePerPair(numPairs);
        var cardTypes = CreateCardPairArray(typePerPair);
        Shuffle(cardTypes);

        return cardTypes;
    }

    // Create a set of size numTypes where each CardType is unique
    private static HashSet<CardType> CreateRandomTypePerPair(int numTypes)
    {
        var result = new HashSet<CardType>();

        // Since HashSet doesn't allow duplicates,
        // this loop will continue until we select
        // required quantity of distinguishable CardType
        while (result.Count < numTypes)
        {
            result.Add(CardType.RandomType());
        }

        return result;
    }

    // Generate a list of CardTypes containing exactly
    // two instance of each CardType in given set
    private static List<CardType> CreateCardPairArray(HashSet<CardType> typePerPair)
    {
        var result = new List<CardType>();

        foreach(var pairType in typePerPair)
        {
            result.Add(pairType);
            result.Add(pairType);
        }

        return result;
    }

    // Shuffle a list with algorithm from stack overflow:
    // https://stackoverflow.com/questions/273313/randomize-a-listt/4262134#4262134
    private static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            var value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
