using Godot;

[GlobalClass]
public partial class Ludopatia : RelicController
{
    public override void OnCardPlayed(Card card)
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        int randomMult = rng.RandiRange(0, 12); 

        playerHand.AddGeneralMult(randomMult);
    }
}
