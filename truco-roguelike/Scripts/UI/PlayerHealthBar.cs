using Godot;

public partial class PlayerHealthBar : Control
{
    [Export] private PlayerController player;

    private ProgressBar progressBar;
    private Label healthLabel;

    float displayedValue;

    public override void _Ready()
    {
        progressBar = GetNode<ProgressBar>("HealthBar");
        healthLabel = GetNode<Label>("HealthText");

        displayedValue = player.health;
    }

    public override void _Process(double delta)
    {
        float target = player.health;

        displayedValue = Mathf.Lerp(displayedValue, target, 8f * (float)delta);

        if (Mathf.Abs(displayedValue - target) < 0.5f)
            displayedValue = target;

        progressBar.Value = displayedValue / player.maxHealth * 100f;
        healthLabel.Text = Mathf.RoundToInt(displayedValue).ToString();
    }
}
