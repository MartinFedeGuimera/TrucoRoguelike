using Godot;

public partial class Enemy : Node
{
    [Export] private GameController gameController;

    [Export] private PlayerController player;

	[Export] private int maxHealth;
	private int health;

	[Export] private int damage;

    [Signal]
    public delegate void TurnEndedEventHandler();

    [Signal]
    public delegate void EnemyDeadEventHandler();

    private bool isDead = false;

    public override void _Ready()
    {
        health = maxHealth;

        player.TurnEnded += DealDamage;

        CalculateMaxHealth();
    }

    public override void _Process(double delta)
    {
        if(health <= 0 && !isDead)
        {
            isDead = true;
            GD.Print("Enemy Killed");

            OnDeath();
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

    private void OnDeath()
    {
        EmitSignal("EnemyDead");
    }

    private void CalculateMaxHealth()
    {
        GD.Print("Enemy Max Healt Calculated!");
        
        maxHealth *= GameData.Instance.round;
        health = maxHealth;
    }

    public int GetHealth() => health;
    public int GetMaxHealth() => maxHealth;
}
