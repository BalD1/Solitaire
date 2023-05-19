using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardLayConditions_Base
{
    public abstract bool CheckLastCard(Card candidateCard, Card lastCardInRow);
    public abstract bool CheckCandidateCard(Card candidateCard);
}
