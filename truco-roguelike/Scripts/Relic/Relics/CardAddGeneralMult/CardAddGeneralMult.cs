using Godot;

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
}
