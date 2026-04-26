using Godot;
using System;

[GlobalClass]
public partial class Cantora : RelicController
{
    public CardSuit suit = CardSuit.None;
    [Export] public int substractValue;

    public override void OnPlayerTurnStarted()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();

        do
        {
            suit = (CardSuit)rng.RandiRange(0, Enum.GetValues(typeof(CardSuit)).Length - 1);
        }
        while( suit == CardSuit.None );

        GD.Print("Suit Selected: " + suit);
    }

    public override void OnCardPlayed(Card card)
    {
        if(card.suit == suit)
        {
            GD.Print("Cantora added perma mult.");
            GD.Print("Suit played: " + card.suit + " Relic suit: " + suit);

            playerHand.AddPermaMult(value);
        }
        else
        {
            GD.Print("Cantora substracted perma mult");
            GD.Print("Suit played: " + card.suit + " Relic suit: " + suit);

            playerHand.AddPermaMult(-substractValue);
        }
    }
}