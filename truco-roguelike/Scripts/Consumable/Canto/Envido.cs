using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Envido : Canto
{
    public int envidoValue = 0;
    
    public override bool OnUse()
    {
        if(hand != null)
        {
            isUsable = true;
        }

        if (!base.OnUse())
            return false;

        envidoValue = CalculateEnvidoValue(hand.GetDrawnCards());

        hand.CreateExternalAttack((int)value + envidoValue);

        return true;
    }

    public int CalculateEnvidoValue(Array<Card> handCards)
    {
        if (handCards.Count == 0)
            return 0;

        Dictionary<CardSuit, Array<int>> suits = new();

        foreach (Card card in handCards)
        {
            if (!suits.ContainsKey(card.suit))
                suits[card.suit] = new Array<int>();

            suits[card.suit].Add(card.envidoValue);
        }

        int bestPairValue = -1;
        int highestSingle = 0;

        foreach (var suit in suits.Keys)
        {
            var values = suits[suit];

            values.Sort();
            values.Reverse();

            highestSingle = Mathf.Max(highestSingle, values[0]);

            if (values.Count >= 2)
            {
                int pairValue = values[0] + values[1] + 20;
                bestPairValue = Mathf.Max(bestPairValue, pairValue);
            }
        }

        if (bestPairValue >= 0)
            return bestPairValue;

        return highestSingle;
    }
}
