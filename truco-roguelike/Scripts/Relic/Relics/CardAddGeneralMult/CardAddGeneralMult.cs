using Godot;
using System;

[GlobalClass]
public partial class CardAddGeneralMult : RelicController
{
    [Export] private Card relicCard;

    public override void OnCardPlayed(Card card)
    {
        if(card.name == relicCard.name)
        {
            playerHand.AddGeneralMult(value);
        }
    }
}
