using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CardSelector : MonoBehaviour
{
    // Event that triggers whenever a valid card selection happens
    public event EventHandler<Card> OnCardSelect;

    // Click detection method (mouse / touch screen / replay)
    public IClickMethod clickMethod = ClickMethodFactory.GetInputMethod();

    // Sound effect to play when a card is selected
    [SerializeField] private AudioClip selectSound;

    private void Awake()
    {
        // Find the component that handles sound effects
        var soundPlayer = GameObject.FindGameObjectWithTag("SoundEffectPlayer").GetComponent<SoundEffectPlayer>();

        // Play sound effect when a card is selected
        OnCardSelect += (sender, arg) => soundPlayer.Play(selectSound);
    }

    // Update is called once per frame
    void Update()
    {
        if (clickMethod.ClickHappened())
        {
            var card = GetSelectedCard(clickMethod.GetClickScreenPosition());
            if (card != null)
            {
                OnCardSelect.Invoke(this, card);
            }
        }
    }

    // Try to find a card instance under clicked screen position.
    // Returns null if nothing was clicked.
    private Card GetSelectedCard(Vector2 screenPosition)
    {
        var ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out var hitinfo))
        {
            var obj = hitinfo.collider.gameObject;
            if (obj.CompareTag("Card"))
            {
                return obj.GetComponent<Card>();
            }
        }

        return null;
    }
}
