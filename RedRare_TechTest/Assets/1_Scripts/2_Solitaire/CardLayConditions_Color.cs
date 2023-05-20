
public class CardLayConditions_Color : CardLayConditions_Base
{
    private bool mustBeSameColor;
    private bool mustBeSameFamily;

    public CardLayConditions_Color(bool _mustBeSameColor, bool _mustBeSameFamily)
    {
        this.mustBeSameColor = _mustBeSameColor;
        this.mustBeSameFamily = _mustBeSameFamily;
    }

    public override bool CheckCandidateCard(Card candidateCard)
    {
        return true;
    }

    public override bool CheckLastCard(Card candidateCard, Card lastCardInRow)
    {
        Card.E_CardFamily candidateFamily = candidateCard.Data.CardFamily;
        Card.E_CardFamily lastCardFamily = lastCardInRow.Data.CardFamily;

        bool areSameFamily = candidateFamily == lastCardFamily;
        bool areSameColor = Card.CompareColor(candidateFamily, lastCardFamily);

        return (mustBeSameColor == areSameColor) && (mustBeSameFamily == areSameFamily);
    }
}
