using Godot;

public partial class PlayerController : Node
{
    [Export] public Enemy enemy;

    private Hand hand;

    [Export] private int maxHealth;
    private int health;

    private int money = 0;

    [Signal]
    public delegate void TurnEndedEventHandler();

    public override void _Ready()
    {
        health = maxHealth;

        hand = GetNode<Hand>("Hand");

        hand.Attack += DealDamage;
        hand.OutOfCards += OnOutOfCards;

        enemy.TurnEnded += StartTurn;
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