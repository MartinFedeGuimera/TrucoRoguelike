using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Cantora : RelicController
{
    public CardSuit suit = CardSuit.Basto;
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

    public override Dictionary<string, object> GetVarsDictionary()
    {
        var vars = new Dictionary<string, object>()
        {
            {"value", value },
            {"suit", suit.ToString()},
            {"substractValue", substractValue}
        }; 

        return vars;    
    }
}