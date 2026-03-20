using Godot;
using Godot.Collections;

[GlobalClass]
public partial class SuitAddMultRelic : RelicController
{
	[Export] public CardSuit multSuit;

    public Card playedCard;

    public override void OnCardPlayed(Card card)
    {
        playedCard = card;

        if (playedCard.suit == multSuit)
        {
            playedCard.mult += value;
        }
    }
}
