using Godot;

[GlobalClass]
public partial class Ludopatia : RelicController
{
    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void OnCardPlayed(Card card)
    {
        rng.Randomize();

        float biasedRandom = Mathf.Pow(rng.Randf(), 2.5f);
        int randomMult = Mathf.RoundToInt(biasedRandom * 12);

        GD.Print("Random Mult: " + randomMult);

        playerHand.AddTempMult(randomMult);
    }
}