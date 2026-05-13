using Godot;

[GlobalClass]
public partial class Ludopatia : RelicController
{
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private int randomMult;

    public override void OnCardPlayed(Card card)
    {
        rng.Randomize();

        float biasedRandom = Mathf.Pow(rng.Randf(), 2.5f);
        randomMult = Mathf.RoundToInt(biasedRandom * value);

        GD.Print("Random Mult: " + randomMult);

        playerHand.AddTempMult(randomMult);
    }
}