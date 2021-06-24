using UnityEngine;

// The group and number of a card (e.g. diamond 2)
public struct CardType
{
    // The shape of this card (e.g., CardGroup.Diamond)
    public CardGroup group;

    // The number of this card (e.g., CardNumber.Two)
    public CardNumber number;

    // Return random combination of group and number
    public static CardType RandomType()
    {
        return new CardType
        {
            group = (CardGroup)Random.Range(0, (int)CardGroup.Length),
            number = (CardNumber)Random.Range(0, (int)CardNumber.Length)
        };
    }

    // Check if two cards are same
    public bool Equals(CardType other)
    {
        return group == other.group && number == other.number;
    }
}
