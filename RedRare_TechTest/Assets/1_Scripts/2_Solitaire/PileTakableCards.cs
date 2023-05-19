using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pile))]
public class PileTakableCards : MonoBehaviour
{
    private Stack<Card> cardsStack;

    public void AddCard(Card card)
    {
        cardsStack.Push(card);
    }
}
