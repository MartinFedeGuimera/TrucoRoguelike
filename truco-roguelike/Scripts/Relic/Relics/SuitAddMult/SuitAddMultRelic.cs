using Godot;
using Godot.Collections;

[GlobalClass]
public partial class SuitAddMultRelic : RelicController
{
	[Export] public CardSuit multSuit;

    public override void OnCardPlayed(Card card)
    {
        if (card.suit == multSuit)
        {
            wasUsed = true;
            card.mult = value;
        }
    }
}
