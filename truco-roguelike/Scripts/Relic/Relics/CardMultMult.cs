using Godot;
using Godot.Collections;

[GlobalClass]
public partial class CardMultMult : RelicController
{
    [Export] private Array<Card> relicCards;

    public override void OnCardPlayed(Card card)
    {
        foreach (var relicCard in relicCards)
        {
            if (relicCard.name == card.name)
            {
                card.mult *= value;
                break;
            }
        }

        base.OnCardPlayed(card);
    }
}
