using Godot;

public partial class GameController : Node2D
{
    [Export] private PlayerController player;
    [Export] private Hand playerHand;

	private RandomNumberGenerator seed = new RandomNumberGenerator();

    [Signal]
    public delegate void CardSelectedEventHandler(CardController card, int mult);

    public override void _Ready()
    {
        seed.Randomize();

        playerHand.CardSelected += OnCardSelected;

        GD.Print("Seed: " + seed.Randi());
    }

    private void OnCardSelected(CardController card, int mult)
    {
        if(IsInstanceValid(card))
            EmitSignal("CardSelected", card, mult);
    }

    public RandomNumberGenerator GetSeed() => seed;
}
