using Godot;

[GlobalClass]
public partial class GameData : Resource
{
	public RandomNumberGenerator seed;
    [Export] public int round;

    public void Save(RandomNumberGenerator seed, int round)
    {
        this.seed = seed;
        this.round = round;
    }
}
