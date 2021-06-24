using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CardSelector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2? clickPosition = GetClickedScreenPosition();
        if (clickPosition.HasValue)
        {
            Debug.Log(clickPosition.Value);
            Card card = GetSelectedCard(clickPosition.Value);

            // If the card can be flipped
            if (card != null && !card.IsFlipping)
            {
                card.Flip();
            }
        }
    }

    private Vector2? GetClickedScreenPosition()
    {
        //if (Input.touchCount > 0)
        //{
        //    return Input.GetTouch(0).position;
        //}
        if (Input.GetMouseButtonDown(0))
        {
            return Input.mousePosition;
        }

        return null;
    }

    // Try to find a card instance under clicked screen position.
    // Returns null if nothing was clicked.
    private Card GetSelectedCard(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hitinfo))
        {
            GameObject obj = hitinfo.collider.gameObject;
            if (obj.tag.Equals("Card"))
            {
                return obj.GetComponent<Card>();
            }
        }

        return null;
    }
}
