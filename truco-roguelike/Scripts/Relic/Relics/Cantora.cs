using Godot;
using System;

[GlobalClass]
public partial class Cantora : RelicController
{
    public CardSuit suit = CardSuit.None;
    [Export] public int substractValue;

    public override void SetUp(Hand hand)
    {
        base.SetUp(hand);

        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();

        suit = (CardSuit)rng.RandiRange(0, Enum.GetValues(typeof(CardSuit)).Length - 1);
    }

    public override void OnPlayerTurnFinished()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();

        suit = (CardSuit)rng.RandiRange(0, Enum.GetValues(typeof(CardSuit)).Length - 1);
    }

    public override void OnCardPlayed(Card card)
    {
        if(card.suit == suit)
        {
            playerHand.AddPermaMult(value);
        }
        else
        {
            playerHand.AddPermaMult(-substractValue);
        }
    }
}