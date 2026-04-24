using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Envido : Canto
{
    public int envidoValue = 0;
    
    public override void OnUse()
    {
        bool hasFlor = hand.hasFlor;

        if(!hasFlor)
        {
            base.OnUse();

            envidoValue = CalculateEnvidoValue(hand.GetDrawnCards());

            hand.CreateExternalAttack((int)value + envidoValue);
        }
    }

    public int CalculateEnvidoValue(Array<Card> handCards)
    {
        bool sameSuitEnvido = false;

        Card firstCard = handCards[0];

        for (int i = 1; i < handCards.Count; i++)
        {
            Card currentCard = handCards[i];

            if (firstCard.suit == currentCard.suit)
            {
                sameSuitEnvido = true;
            }
        }

        int firstEnvidoValue = 0;
        int secondEnvidoValue = 0;

        if (sameSuitEnvido)
        {
            foreach (Card card in handCards)
            {
                int value = card.envidoValue;

                if (value > firstEnvidoValue)
                {
                    secondEnvidoValue = firstEnvidoValue;
                    firstEnvidoValue = value;
                }
                else if (value > secondEnvidoValue)
                {
                    secondEnvidoValue = value;
                }
            }

            return firstEnvidoValue + secondEnvidoValue + 10;
        }
        else
        {
            foreach (Card card in handCards)
            {
                if (card.envidoValue > firstEnvidoValue)
                {
                    firstEnvidoValue = card.envidoValue;
                }
            }

            return firstEnvidoValue;
        }
    }
}
