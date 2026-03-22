using Godot;

public partial class Enemy : Node
{
    [Export] public PlayerController player;

	[Export] public int maxHealth;
	public int health;

	[Export] private int damage;

    [Signal]
    public delegate void TurnEndedEventHandler();

    private bool isDead = false;

    public override void _Ready()
    {
        health = maxHealth;

        player.TurnEnded += DealDamage;
    }

    public override void _Process(double delta)
    {
        if(health <= 0 && !isDead)
        {
            isDead = true;
            GD.Print("Enemy Killed");
        }
    }

    private void DealDamage()
    {
        if(!isDead)
        {
            player.TakeDamage(damage);
            GD.Print("Damage Taken: " + damage);

            EmitSignal("TurnEnded");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        GD.Print("Enemy Health: " + health);
    }
}
