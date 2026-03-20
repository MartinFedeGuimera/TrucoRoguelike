using Godot;
using System;

public partial class PlayerHealthBar : Control
{
	[Export] private PlayerController player;

	private ProgressBar progressBar;
	private Label healthLabel;

    public override void _Ready()
    {
        progressBar = GetNode<ProgressBar>("HealthBar");
        healthLabel = GetNode<Label>("HealthText");
    }

    public override void _Process(double delta)
    {
        progressBar.Value = player.health * 100 / player.maxHealth;

        if (player.health > 0)
        {
            healthLabel.Text = player.health.ToString();
        }
        else
        {
            healthLabel.Text = "0";
        }
    }
}
