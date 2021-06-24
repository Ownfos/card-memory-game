using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CardSelector : MonoBehaviour
{
    // Event that triggers whenever a valid card selection happens
    public event EventHandler<Card> OnCardSelect;

    // Click detection method (mouse vs touch screen)
    private IClickMethod clickMethod = new ClickByMouse();

    private void Start()
    {
        OnCardSelect += (sender, card) => card.Flip();
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
            if (obj.tag.Equals("Card"))
            {
                return obj.GetComponent<Card>();
            }
        }

        return null;
    }
}
