using Godot;

public partial class GameData : Node
{
    public static GameData Instance { get; private set; }

    public RandomNumberGenerator seed;

    public int round;

    public override void _Ready()
    {
        GD.Print("GameData Ready");

        Instance = this;
        seed = new RandomNumberGenerator();
        round = 1;
    }

    public void RestartData()
    {
        round = 1;
        seed = new RandomNumberGenerator();
    }
}