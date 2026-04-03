using Godot;
using System.Collections;

public partial class GameController : Node2D
{
    [Export] private PlayerController player;
    [Export] private Hand playerHand;

    [Export] private Enemy enemy;

    [Export] private GameData gameData;

    [Export] private int roundWinPrize;

    private int round = 0;

    private bool gameLoaded = false;

	private RandomNumberGenerator seed = new RandomNumberGenerator();

    [Signal]
    public delegate void CardSelectedEventHandler(CardController card);

    [Signal]
    public delegate void LoadingSceneEventHandler();

    [Signal]
    public delegate void DataLoadedEventHandler();

    public override void _Ready()
    {
        if(gameData.round != 0)
        {
            LoadData();
        }

        round++;
        GD.Print("Current Round: " + round);


        if(gameData.round == 1)
        {
            seed.Randomize();
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

        roundWinPrize = (int)(roundWinPrize * 1.5f) ;

        LoadShop();
    }

    private void LoadShop()
    {
        EmitSignal("LoadingScene");

        SaveData();

        SceneManager.Instance.Load("res://Scenes/Screens/shop.tscn");
    }

    private void SaveData()
    {
        gameData.Save(seed, round);
        GD.Print("Game Data Saved");
    }

    private void LoadData()
    {
        seed = gameData.seed;
        round = gameData.round;
        GD.Print("Game Data Loaded");

        EmitSignal("DataLoaded");
    }

    public RandomNumberGenerator GetSeed() => seed;
    public int GetRound() => round;
    public bool IsGameLoaded() => gameLoaded;
}
