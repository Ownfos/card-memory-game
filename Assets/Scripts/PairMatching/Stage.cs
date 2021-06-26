using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A stage is an object with card prefabs as child object.
// Each card 
public class Stage : MonoBehaviour
{
    // Event that triggers when all cards in this stage is correctly flipped
    public event EventHandler OnStageComplete;

    // Event that triggers when two cards of same type are consecutively flipped
    public event EventHandler OnPairMatch;

    // Event that triggers when two cards of different types are consecutively flipped
    public event EventHandler OnPairMismatch;

    // The Card components of each card in this stage
    private List<Card> cards = new List<Card>();

    // The Card component of lastly flipped card instances
    private Card firstFlippedCard = null;
    private Card secondFlippedCard = null;

    // Scores to add/deduct on pair match/mismatch
    [SerializeField] private int _matchReward;
    [SerializeField] private int _mismatchPanelty;
    public int MatchReward { get { return _matchReward; } }
    public int MismatchPanelty { get { return _mismatchPanelty; } }

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

    // Activate gameObject and make the stage pop in (scale: 0 -> 1)
    public void Activate(LeanTweenType stageSwapEffect)
    {
        gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 1.0f)
                .setEase(stageSwapEffect);
    }

    // Make the stage pop out (scale: 1 -> 0) and deactivate gameObject
    public void Deactivate(LeanTweenType stageSwapEffect)
    {
        transform.localScale = Vector3.one;
        LeanTween.scale(gameObject, Vector3.zero, 1.0f)
            .setOnComplete(() => gameObject.SetActive(false))
            .setEase(stageSwapEffect);
    }

    // Setup cards according to configuration strategy
    // and register card selection event handler
    public void Initialize(ICardConfiguration configuration)
    {
        // Prepare cards
        FindAllCardsInStage();
        ConfigureCards(configuration.GetCardConfiguration(cards.Count));

        // Record stage configuration to ReplayManager
        RecordStageConfiguration();

        // Register event handler
        RegisterCardSelectEventHandler();
    }

    // If replay is not running, record current configuration to ReplayManager.
    // This data will be used to replicate same configuration when we actually replay this game.
    private void RecordStageConfiguration()
    {
        var replayManager = GameObject.FindGameObjectWithTag("ReplayManager").GetComponent<ReplayManager>();
        if (!replayManager.IsReplayRunning)
        {
            replayManager.RecordStageConfiguration(GetCurrentConfiguration());
        }
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
        if(card.State == CardState.Backface)
        {
            card.Flip();
        }
    }

    // Loop over child objects and save their Card components to variable 'cards'
    private void FindAllCardsInStage()
    {
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
        for(int i = 0; i < cards.Count; ++i)
        {
            cards[i].Initialize(cardConfiguration[i]);
            cards[i].OnFlip += OnFlipHandler;
        }
    }

    // Event handler for card flip event
    private void OnFlipHandler(object sender, Card card)
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

    // After some delay, flip back each card
    private void OnPairMismatchHandler(object sender, EventArgs e)
    {
        StartCoroutine(FlipBack(firstFlippedCard));
        StartCoroutine(FlipBack(secondFlippedCard));
    }

    // Returns true if all pairs are matched
    private bool CheckStageCompletion()
    {
        // If any of the cards are not yet flipped correctly, return false.
        foreach (var card in cards)
        {
            if (card.State != CardState.FrontfaceCorrect)
            {
                return false;
            }
        }

        return true;
    }

    // Flip the incorrectly matched card pair back to backface.
    private IEnumerator FlipBack(Card card)
    {
        // Make sure flip animation (if any) is done
        yield return new WaitForSeconds(card.FlipAnimationLength);

        card.Flip();
    }
}
