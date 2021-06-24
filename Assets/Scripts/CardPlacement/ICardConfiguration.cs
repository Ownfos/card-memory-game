using System.Collections.Generic;

// This is an interface for card configuration generators.
//
// Card configuration refers to the array of CardType,
// which is directly mapped to the Card instances in a stage.
public interface ICardConfiguration
{
    // Generate a list of CardType where each of the
    // different CardType appears exactly twice.
    List<CardType> GetCardConfiguration(int numCards);
}
