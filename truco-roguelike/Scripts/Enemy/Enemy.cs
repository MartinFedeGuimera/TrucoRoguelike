using Godot;
using System;

public partial class Enemy : Node
{
    [Export] public PlayerController player;

	[Export] private int maxHealth;
	private int health;

	[Export] private int damage;

    [Signal]
    public delegate void TurnEndedEventHandler();

    public override void _Ready()
    {
        health = maxHealth;

        player.TurnEnded += DealDamage;
    }

    public override void _Process(double delta)
    {
        if(health <= 0)
        {
            GD.Print("Enemy Killed");
        }
    }

    private void DealDamage()
    {
        player.TakeDamage(damage);
        GD.Print("Damage Taken: " + damage);

        EmitSignal("TurnEnded");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
