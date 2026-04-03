using Godot;
using System;

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
}
