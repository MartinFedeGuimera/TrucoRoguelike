using Godot;

public partial class PlayerController : Node
{
    [Export] private int maxHealth;
    private int health;

    private int money = 0;

    public override void _Ready()
    {

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {

    }
}