using Godot;

[GlobalClass]
public partial class SuitAddGeneralMult : RelicController
{
	[Export] public CardSuit multSuit;

    public override void OnCardPlayed(Card card)
    {
        if (card.suit == multSuit)
        {
            wasUsed = true;
            playerHand.AddGeneralMult(value);
        }
    }
}
