using Godot;

[GlobalClass]
public partial class CardAddMult : RelicController
{
    [Export] private Card relicCard;

    public override void OnCardPlayed(Card card)
    {
        if (card.name == relicCard.name)
        {
            wasUsed = true;
            card.mult += value;
        }
    }
}
