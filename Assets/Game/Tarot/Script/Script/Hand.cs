using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetHand(List<Card> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            Card card = cards[i];

            card.transform.SetParent(transform, true);

            //card.SpreadOut();
        }
    }
}
