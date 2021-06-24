using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedConfiguration : ICardConfiguration
{
    private List<CardType> cardConfiguration;

    public FixedConfiguration(List<CardType> cardConfiguration)
    {
        this.cardConfiguration = cardConfiguration;
    }

    public List<CardType> GetCardConfiguration(int numCards)
    {
        return cardConfiguration;
    }
}
