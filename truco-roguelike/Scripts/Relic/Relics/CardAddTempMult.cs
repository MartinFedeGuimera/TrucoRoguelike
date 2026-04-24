using Godot;
using Godot.Collections;

[GlobalClass]
public partial class CardAddTempMult : RelicController
{
    [Export] private Array<Card> relicCards; 

    public override void OnCardPlayed(Card card)
    {
        foreach(var relicCard in relicCards)
        {
            if(relicCard.name == card.name)
            {
                playerHand.AddTempMult(value);
                break;
            }
        }

        base.OnCardPlayed(card);
    }
}
