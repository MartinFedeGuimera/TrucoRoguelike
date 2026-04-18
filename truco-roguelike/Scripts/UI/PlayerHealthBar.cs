using Godot;

public partial class PlayerHealthBar : Control
{
    private ProgressBar progressBar;
    private Label healthLabel;

    float displayedValue;

    public override void _Ready()
    {
        progressBar = GetNode<ProgressBar>("HealthBar");
        healthLabel = GetNode<Label>("HealthText");

        displayedValue = PlayerData.Instance.health;
    }

    public override void _Process(double delta)
    {
        float target = PlayerData.Instance.health;

        displayedValue = Mathf.Lerp(displayedValue, target, 8f * (float)delta);

        if (Mathf.Abs(displayedValue - target) < 0.5f)
            displayedValue = target;

        progressBar.Value = displayedValue / PlayerData.Instance.maxHealth * 100f;
        healthLabel.Text = Mathf.RoundToInt(displayedValue).ToString();
    }
}
