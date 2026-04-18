using Godot;
using Godot.Collections;

public partial class PlayerController : Node
{
    [Export] private GameController gameController;
    [Export] public Enemy enemy;

    private Hand hand;

    [Signal]
    public delegate void TurnEndedEventHandler();

    [ExportGroup("Scenes")]
    [Export] private PackedScene relicScene;
    [Export] private PackedScene consumableScene;

    [ExportGroup("UI")]
    [Export] private Node relicsParent;
    [Export] private Node consumablesParent;

    public override void _Ready()
    {
        hand = GetNode<Hand>("Hand");

        hand.Attack += DealDamage;
        hand.OutOfCards += OnOutOfCards;

        enemy.TurnEnded += StartTurn;

        gameController.DataLoaded += LoadData;

        UpdateUI();
    }

    public override void _Process(double delta)
    {
        if (PlayerData.Instance.health <= 0)
            GD.Print("Game Over!");
    }

    // ---------------- DAMAGE ----------------

    private void DealDamage(int damage)
    {
        enemy.TakeDamage(damage);
        GD.Print("Damage Dealed: " + damage);
    }

    public void TakeDamage(int damage)
    {
        PlayerData.Instance.health -= damage;
    }

    // ---------------- TURN ----------------

    private void StartTurn()
    {
        hand.canStart = true;
        GD.Print("Health: " + PlayerData.Instance.health);
    }

    // ---------------- MONEY ----------------

    public void AddMoney(int addedMoney)
    {
        PlayerData.Instance.money += addedMoney;
    }

    // ---------------- UI ----------------

    private void UpdateUI()
    {
        foreach (Node child in relicsParent.GetChildren())
            child.QueueFree();

        foreach (var relic in PlayerData.Instance.relics)
        {
            RelicView relicNode = relicScene.Instantiate<RelicView>();
            relicNode.SetUp(relic);
            relicsParent.AddChild(relicNode);
        }

        foreach (Node child in consumablesParent.GetChildren())
            child.QueueFree();

        foreach (var consumable in PlayerData.Instance.consumables)
        {
            ConsumableController controller =
                consumableScene.Instantiate<ConsumableController>();

            controller.SetUp(consumable, hand);
            consumablesParent.AddChild(controller);
        }
    }

    // ---------------- LOAD ----------------

    public void LoadData()
    {
        GD.Print("Player Data Loaded");
        UpdateUI();
    }

    private void OnOutOfCards()
    {
        EmitSignal(SignalName.TurnEnded);
    }
}