using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    // The card configuration strategy to use.
    // By default, we randomly select and place card pairs.
    public ICardConfiguration configuration = new RandomConfiguration();

    // Event that triggers when all cards in this stage is correctly flipped
    public event EventHandler OnStageComplete;

    // The Card components of each card in this stage
    private List<Card> cards;

    // The Card component of lastly flipped card instance
    private Card lastFlippedCard = null;

    // Test code that initialize stage rightaway after creation.
    // In real application, Initialize should be called manually after
    // configuration strategy is correctly set.
    void Awake()
    {
        //Initialize();
    }

    // Setup cards according to configuration strategy
    // and register card selection event handler
    public void Initialize()
    {
        // Prepare cards
        FindAllCardsInStage();
        ConfigureCards(configuration.GetCardConfiguration(cards.Count));

        // Register event handler
        RegisterCardSelectEventHandler();
    }

    // Register event handler for card selection event
    private void RegisterCardSelectEventHandler()
    {
        var cardSelector = GameObject.FindGameObjectWithTag("CardSelector").GetComponent<CardSelector>();
        cardSelector.OnCardSelect += OnCardSelectHandler;
    }

    // Event handler for card selection event
    private void OnCardSelectHandler(object sender, Card card)
    {
        // Flip the card if it's showing back face and not moving
        if(!card.IsFlipAnimRunning)
        {
            // New card is selected
            if (!card.IsFlipped)
            {
                card.Flip();
            }
            // The card was relelected
            else if (card == lastFlippedCard)
            {
                lastFlippedCard = null;
                card.Flip();
            }
        }
    }

    // Loop over child objects and save their Card components to variable 'cards'
    private void FindAllCardsInStage()
    {
        cards = new List<Card>();
        for (int i = 0; i < transform.childCount; ++i)
        {
            cards.Add(transform.GetChild(i).GetComponent<Card>());
        }
    }

    // Loop over child objects and initialize them
    // with given configuration (CardType for each instance).
    // Given parameter's length should match with the number
    // of child objects (cards) in this stage.
    private void ConfigureCards(List<CardType> cardConfiguration)
    {
        // Find the component that gives texture for each type
        var frontfaceImages = GameObject.FindGameObjectWithTag("FrontFaceImages").GetComponent<CardFrontFaceImages>();
        
        // Loop over card instances
        for(int i = 0; i < cards.Count; ++i)
        {
            // Initialize
            var type = cardConfiguration[i];
            var texture = frontfaceImages.GetFrontFaceTexture(type);
            cards[i].Initialize(type, texture);

            // Attach flip event handler
            cards[i].OnFlip += OnFlipHandler;
        }
    }

    // Event handler for card flip event
    private void OnFlipHandler(object sender, Card card)
    {
        if (card.IsFlipped)
        {
            // First card is flipped
            if (lastFlippedCard == null)
            {
                lastFlippedCard = card;
            }
            // Second card is flipped
            else
            {
                if (lastFlippedCard.Type.Equals(card.Type))
                {
                    HandleMatch(lastFlippedCard, card);
                }
                else
                {
                    HandleMismatch(lastFlippedCard, card);
                }
                lastFlippedCard = null;
            }
        }
    }

    // Mark flipped pair of card as correct and handle stage completion
    private void HandleMatch(Card card1, Card card2)
    {
        card1.MarkAsCorrectlyFlipped();
        card2.MarkAsCorrectlyFlipped();

        if(CheckStageCompletion())
        {
            OnStageComplete?.Invoke(this, EventArgs.Empty);
        }
    }

    // Returns true if all cards are correctly flipped
    private bool CheckStageCompletion()
    {
        foreach(var card in cards)
        {
            if (!card.IsCorrectlyFlipped)
            {
                return false;
            }
        }

        return true;
    }

    // After some delay, flip back each card
    private void HandleMismatch(Card card1, Card card2)
    {
        StartCoroutine(FlipBack(card1));
        StartCoroutine(FlipBack(card2));
    }

    // Make sure that current flip animation is done
    // and then flip the card back
    private IEnumerator FlipBack(Card card)
    {
        yield return new WaitForSeconds(card.FlipAnimationLength);
        card.Flip();
    }
}
