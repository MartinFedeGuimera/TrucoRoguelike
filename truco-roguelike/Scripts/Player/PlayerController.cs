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

    [Export] private Node relicsParent;
    [Export] private PackedScene relicScene;
    private bool relicsLoaded = false;

    [ExportGroup("Consumables")]
    [Export] private Array<Consumable> consumables = new Array<Consumable>();
    [Export] private PackedScene consumableScene;
    [Export] private Node consumablesParent;

    public override void _Ready()
    {
        GD.Print(data.GetInstanceId());

        health = maxHealth;

        hand = GetNode<Hand>("Hand");

        hand.Attack += DealDamage;
        hand.OutOfCards += OnOutOfCards;

        enemy.TurnEnded += StartTurn;

        gameController.LoadingScene += SaveData;

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
        money = data.money;
        relics = data.relics;
        consumables = data.consumables;
        maxHealth = data.maxHealth;
        health = data.health;

        GD.Print("Player Data Loaded");

        UpdateUI();
    }

    private void OnOutOfCards() => EmitSignal("TurnEnded");
}