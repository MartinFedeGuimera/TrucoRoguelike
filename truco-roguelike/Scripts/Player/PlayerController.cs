using Godot;
using Godot.Collections;

public partial class PlayerController : Node
{
    [Export] public Enemy enemy;

    private Hand hand;

    [Export] public int maxHealth;
    public int health;

    private int money = 0;

    [Signal]
    public delegate void TurnEndedEventHandler();

    [ExportGroup("Relics")]
    [Export] public Array<RelicController> relics;

    [Export] private Node relicsParent;
    [Export] private PackedScene relicScene; 
    private bool relicsLoaded = false;

    [ExportGroup("Consumables")]
    [Export] private Array<Consumable> consumables;
    [Export] private PackedScene consumableScene;
    [Export] private Node consumablesParent;

    public override void _Ready()
    {
        health = maxHealth;

        hand = GetNode<Hand>("Hand");

        hand.Attack += DealDamage;
        hand.OutOfCards += OnOutOfCards;

        enemy.TurnEnded += StartTurn;

        foreach (var relic in relics)
        {
            RelicView relicNode = relicScene.Instantiate<RelicView>();

            relicNode.SetUp(relic);

            relicsParent.AddChild(relicNode);
        }
        GD.Print("Relics Loaded!");

        foreach(var consumable in consumables)
        {
            ConsumableController consumableController = consumableScene.Instantiate<ConsumableController>();

            consumableController.SetUp(consumable, hand);

            consumablesParent.AddChild(consumableController);
        }    
    }

    public override void _Process(double delta)
    {
        if(health <= 0)
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

        GD.Print("Health: " +  health);
    }

    private void OnOutOfCards() => EmitSignal("TurnEnded");
}