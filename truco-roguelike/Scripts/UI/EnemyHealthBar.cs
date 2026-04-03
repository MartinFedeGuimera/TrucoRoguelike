using Godot;
using System;

public partial class EnemyHealthBar : Control
{
	[Export] private Enemy enemy;

	private ProgressBar progressBar;
	private Label healthLabel;

    public override void _Ready()
    {
        progressBar = GetNode<ProgressBar>("HealthBar");
        healthLabel = GetNode<Label>("HealthText");
    }

    public override void _Process(double delta)
    {
        progressBar.Value = enemy.GetHealth() * 100 / enemy.GetMaxHealth();

        if (enemy.GetHealth() > 0)
        {
            healthLabel.Text = enemy.GetHealth().ToString();
        }
        else
        {
            healthLabel.Text = "0";
        }
    }
}
