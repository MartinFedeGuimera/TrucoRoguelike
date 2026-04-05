using Godot;

public partial class EnemyHealthBar : Control
{
    [Export] private Enemy enemy;

    private ProgressBar progressBar;
    private Label healthLabel;

    float displayedValue;

    public override void _Ready()
    {
        progressBar = GetNode<ProgressBar>("HealthBar");
        healthLabel = GetNode<Label>("HealthText");

        displayedValue = enemy.GetHealth();
    }

    public override void _Process(double delta)
    {
        float target = enemy.GetHealth();

        displayedValue = Mathf.Lerp(displayedValue, target, 8f * (float)delta);

        if (Mathf.Abs(displayedValue - target) < 0.5f)
            displayedValue = target;

        progressBar.Value = displayedValue / enemy.GetMaxHealth() * 100f;
        healthLabel.Text = Mathf.RoundToInt(displayedValue).ToString();
    }
}