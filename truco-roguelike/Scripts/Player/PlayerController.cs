using Godot;
using Godot.Collections;

public partial class PlayerController : Node
{
    [Export] private GameController gameController;
    [Export] public Enemy enemy;
    [Export] private DescriptionController descriptionController;

    private Hand hand;

    [Signal]
    public delegate void TurnEndedEventHandler();
    [Signal]
    public delegate void PlayerDeadEventHandler();

    [ExportGroup("Scenes")]
    [Export] private PackedScene relicScene;
    [Export] private PackedScene consumableScene;

    [ExportGroup("UI")]
    [Export] private Node relicsParent;
    [Export] private Node consumablesParent;

    [ExportGroup("Debugging")]
    [Export] private Array<RelicController> relicsAdded = new Array<RelicController>();
    [Export] private Array<Consumable> consumablesAdded = new Array<Consumable>();

    public override void _Ready()
    {
        hand = GetNode<Hand>("Hand");

        descriptionController.SetUp();

        hand.Attack += DealDamage;
        hand.OutOfCards += OnOutOfCards;

        enemy.TurnEnded += StartTurn;

        if(GameData.Instance.round == 1)
        {
            foreach (var relic in relicsAdded)
            {
                PlayerData.Instance.relics.Add(relic);
            }
            foreach (var consumable in consumablesAdded)
            {
                PlayerData.Instance.consumables.Add(consumable);
            }
        }

        UpdateUI();
    }

    public override void _Process(double delta)
    {
        if (PlayerData.Instance.health <= 0)
        {
            EmitSignal("PlayerDead");
        }
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
            relicNode.SetUp(relic, descriptionController);
            relicsParent.AddChild(relicNode);
        }

        foreach (Node child in consumablesParent.GetChildren())
            child.QueueFree();

        foreach (var consumable in PlayerData.Instance.consumables)
        {
            ConsumableController controller =
                consumableScene.Instantiate<ConsumableController>();

            controller.SetUp(consumable, hand, descriptionController);
            consumablesParent.AddChild(controller);
        }
    }

    private void OnOutOfCards()
    {
        EmitSignal(SignalName.TurnEnded);
    }
}