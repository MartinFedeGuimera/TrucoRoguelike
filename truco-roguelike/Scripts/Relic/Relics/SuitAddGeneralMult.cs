using Godot;
using System.Collections.Generic;

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

    public override Dictionary<string, object> GetVarsDictionary()
    {
        var vars = new Dictionary<string, object>()
        {
            {"value", value},
            {"multSuit", multSuit.ToString()}
        };

        return vars;
    }
}
