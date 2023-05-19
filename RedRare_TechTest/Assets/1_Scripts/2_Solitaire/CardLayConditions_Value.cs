using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayConditions_Value : CardLayConditions_Base
{
    private bool descendingValues;
    private int mustBeValue = -1;

    public CardLayConditions_Value(bool _descendingValues, int _mustBeValue = -1)
    {
        this.descendingValues = _descendingValues;
        this.mustBeValue = _mustBeValue;
    }

    public override bool CheckCandidateCard(Card candidateCard)
    {
        if (mustBeValue >= 0) return candidateCard.Data.Value == mustBeValue;
        return true;
    }

    public override bool CheckLastCard(Card candidateCard, Card lastCardInRow)
    {
        int candidateValue = candidateCard.Data.Value;
        int lastCardValue = lastCardInRow.Data.Value;

        if (mustBeValue >= 0) return candidateValue == mustBeValue; 

        // if descending : we want the candidate value to be lastcard's -1
        // else : we want the candidate value to be lastcard's +1
        return descendingValues ? candidateValue == lastCardValue - 1 :
                                  candidateValue == lastCardValue + 1;
    }
}
