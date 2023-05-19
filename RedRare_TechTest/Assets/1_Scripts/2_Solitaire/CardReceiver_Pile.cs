using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReceiver_Pile : CardReceiver
{
    public override List<Card> GetEveryCardsTo(Card card)
    {
        List<Card> list = new List<Card>();

        if (card == null) return list;

        list.Add(GetNextCard());

        return list;
    }
}
