using Godot;
using Godot.Collections;

public partial class PlayerController : Node
{
    [Export] private PlayerData data;

    [Export] private GameController gameController;
    [Export] public Enemy enemy;

    private Hand hand;

    [Export] public int maxHealth;
    public int health;

    private int money = 0;

    [Signal]
    public delegate void TurnEndedEventHandler();

    [ExportGroup("Relics")]
    [Export] public Array<RelicController> relics = new Array<RelicController>();
    [Export] private PackedScene relicScene;
    [Export] private int maxRelics;

    [ExportGroup("Consumables")]
    [Export] private Array<Consumable> consumables = new Array<Consumable>();
    [Export] private PackedScene consumableScene;
    [Export] private int maxConsumables;

    [ExportGroup("UI")]
    [Export] private Node relicsParent;
    [Export] private Node consumablesParent;

    public override void _Ready()
    {
        health = maxHealth;

        hand = GetNode<Hand>("Hand");

        hand.Attack += DealDamage;
        hand.OutOfCards += OnOutOfCards;

        enemy.TurnEnded += StartTurn;

        gameController.LoadingScene += SaveData;
        gameController.DataLoaded += LoadData;

        UpdateUI();
    }

    public override void _Process(double delta)
    {
        if (health <= 0)
        {
            GD.Print("Game Over!");
        }
    }

    private void DealDamage(int damage)
    {
        enemy.TakeDamage(damage);

        GD.Print("Damage Dealed: " + damage);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void StartTurn()
    {
        hand.canStart = true;

        GD.Print("Health: " + health);
    }

    public void AddMoney(int adddedMoney)
    {
        money += adddedMoney;

        SaveData();
    }

    private void UpdateUI()
    {
        if (relics != null)
        {
            foreach (var relic in relics)
            {
                RelicView relicNode = relicScene.Instantiate<RelicView>();

                relicNode.SetUp(relic);

                relicsParent.AddChild(relicNode);
            }
        }

        if (consumables != null)
        {
            foreach (var consumable in consumables)
            {
                ConsumableController consumableController = consumableScene.Instantiate<ConsumableController>();

                consumableController.SetUp(consumable, hand);

                consumablesParent.AddChild(consumableController);
            }
        }
    }

    private void SaveData()
    {
        data.Save(money, relics, consumables, maxHealth, health);
        GD.Print("Player Data Saved");
    }

    public void LoadData()
    {
        if(gameController.GetRound() > 0)
        {
            money = data.money;
            relics = data.relics;
            maxRelics = data.maxRelics;
            consumables = data.consumables;
            maxConsumables = data.maxConsumables;
            maxHealth = data.maxHealth;
            health = data.health;

            GD.Print("Player Data Loaded");

            UpdateUI();
        }
    }

    private void OnOutOfCards() => EmitSignal("TurnEnded");
}
