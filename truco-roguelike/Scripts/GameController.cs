using Godot;

public partial class GameController : Node2D
{
    [Export] private PlayerController player;
    [Export] private Hand playerHand;

    [Export] private Enemy enemy;

    [Export] private int roundWinPrize;

    private bool gameLoaded = false;

	private RandomNumberGenerator seed = new RandomNumberGenerator();

    [Signal]
    public delegate void CardSelectedEventHandler(CardController card);

    [Signal]
    public delegate void LoadingSceneEventHandler();

    public override void _Ready()
    {

        GameData.Instance.round++;
        GD.Print("Current Round: " + GameData.Instance.round);


        if(GameData.Instance.round == 1)
        {
            seed.Randomize();
            GameData.Instance.seed = seed;
        }

        playerHand.CardSelected += OnCardSelected;
        enemy.EnemyDead += OnEnemyKilled;

        GD.Print("Seed: " + seed.Randi());
    }

    private void OnCardSelected(CardController card )
    {
        if(IsInstanceValid(card))
            EmitSignal("CardSelected", card);
    }

    private void OnEnemyKilled()
    {
        player.AddMoney(roundWinPrize);

        roundWinPrize = (int)(roundWinPrize * 1.75f) ;

        LoadShop();
    }

    private void LoadShop()
    {
        EmitSignal("LoadingScene");

        SceneManager.Instance.Load("res://Scenes/Screens/shop.tscn");
    }

    public RandomNumberGenerator GetSeed() => seed;
    public bool IsGameLoaded() => gameLoaded;
}
