using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    // The card configuration strategy to use.
    // By default, we randomly select and place card pairs.
    public ICardConfiguration configuration = new RandomConfiguration();

    // The Card components of each card in this stage
    private List<Card> cards;

    // Test code that initialize stage rightaway after creation.
    // In real application, Initialize should be called manually after
    // configuration strategy is correctly set.
    void Awake()
    {
        Initialize();
    }

    // Start is called before the first frame update
    public void Initialize()
    {
        FindAllCardsInStage();
        ConfigureCards(configuration.GetCardConfiguration(cards.Count));
        RegisterCardSelectEvent();
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
    private void OnFlipHandler(object sender, EventArgs e)
    {
        var card = (Card)sender;
        if (card.IsFlipped)
        {
            Debug.Log("Flip event occured");
        }
    }
}
