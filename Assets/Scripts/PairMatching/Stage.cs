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

    // Event that triggers when two cards of same type are consecutively flipped
    public event EventHandler OnPairMatch;

    // Event that triggers when two cards of different types are consecutively flipped
    public event EventHandler OnPairMismatch;

    // The Card components of each card in this stage
    private List<Card> cards;

    // The Card component of lastly flipped card instances
    private Card firstFlippedCard = null;
    private Card secondFlippedCard = null;


    void Awake()
    {
        // Find the component that handles score
        var scoreManager = GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>();

        // Find the component that handles sound effects
        var soundPlayer = GameObject.FindGameObjectWithTag("SoundEffectPlayer").GetComponent<SoundEffectPlayer>();

        // Attach handlers for pair match event
        OnPairMatch += OnPairMatchHandler;
        OnPairMatch += scoreManager.OnPairMatchHandler;
        OnPairMatch += (sender, arg) => soundPlayer.Play(SoundEffect.PairMatch);

        // Attach handlers for pair mismatch event
        OnPairMismatch += OnPairMismatchHandler;
        OnPairMismatch += scoreManager.OnPairMismatchHandler;
        OnPairMismatch += (sender, arg) => soundPlayer.Play(SoundEffect.PairMismatch);

        // Attach handlers for stage completion event
        OnStageComplete += (sender, arg) => soundPlayer.Play(SoundEffect.StageComplete);
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

    // Return the list of CardType for all cards in this stage.
    // Used to create a FixedConfiguration for replay feature.
    public List<CardType> GetCurrentConfiguration()
    {
        var result = new List<CardType>();
        foreach(var card in cards)
        {
            result.Add(card.Type);
        }
        return result;
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
        if(!card.IsFlipAnimRunning && !card.IsFlipped)
        {
            card.Flip();
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
            if (firstFlippedCard == null)
            {
                firstFlippedCard = card;
            }
            // Second card is flipped
            else
            {
                // Handle events
                secondFlippedCard = card;
                if (firstFlippedCard.Type.Equals(card.Type))
                {
                    OnPairMatch.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    OnPairMismatch.Invoke(this, EventArgs.Empty);
                }

                // Reset card references
                firstFlippedCard = null;
                secondFlippedCard = null;
            }
        }
    }

    // Mark flipped pair of card as correct and handle stage completion
    private void OnPairMatchHandler(object sender, EventArgs e)
    {
        firstFlippedCard.MarkAsCorrectlyFlipped();
        secondFlippedCard.MarkAsCorrectlyFlipped();

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
    private void OnPairMismatchHandler(object sender, EventArgs e)
    {
        StartCoroutine(FlipBack(firstFlippedCard));
        StartCoroutine(FlipBack(secondFlippedCard));
    }

    // Make sure that current flip animation is done
    // and then flip the card back
    private IEnumerator FlipBack(Card card)
    {
        yield return new WaitForSeconds(card.FlipAnimationLength);
        card.Flip();
    }
}
