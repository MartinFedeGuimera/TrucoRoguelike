using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Judas : RelicController
{
    [Export] private Card relicCard;
    [Export] private int secondValue;

    public override void OnCardPlayed(Card card)
    {
        if (card.name == relicCard.name)
        {
            card.mult *= value;
            playerHand.AddGeneralMult(-secondValue);
        }
    }

    public override Dictionary<string, object> GetVarsDictionary()
    {
        var vars = new Dictionary<string, object>()
        {
            {"value", value},
            {"relicCard", relicCard.name},
            {"secondValue", secondValue}
        };

        return vars;
    }
}
