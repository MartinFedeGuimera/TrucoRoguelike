using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class CardAddGeneralMult : RelicController
{
    [Export] private Card relicCard;

    public override void OnCardPlayed(Card card)
    {
        if(card.name == relicCard.name)
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
            {"relicCard", relicCard.name}
        };

        return vars;
    }
}
