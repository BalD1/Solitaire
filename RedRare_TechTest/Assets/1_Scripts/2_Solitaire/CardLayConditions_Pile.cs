using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLayConditions_Pile : CardLayConditions_Base
{
    public override bool CheckCandidateCard(Card candidateCard) => false;

    // we never want to lay cards in the pile
    public override bool CheckLastCard(Card candidateCard, Card lastCardInRow)
    {
        return false;
    }
}
