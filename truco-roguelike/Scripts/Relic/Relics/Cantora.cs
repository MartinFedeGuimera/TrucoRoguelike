using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Cantora : RelicController
{
    public CardSuit suit = CardSuit.Basto;
    [Export] public int substractValue;
    private int multAdded = 0;

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
            playerHand.AddPermaMult(value);

            multAdded += value;
        }
        else
        {
            GD.Print("Cantora substracted perma mult");
            playerHand.AddPermaMult(-substractValue);

            multAdded -= substractValue;
            if(multAdded < 0)
                multAdded = 0;
        }
    }

    public override void OnSell()
    {
        playerHand.AddPermaMult(-multAdded);
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