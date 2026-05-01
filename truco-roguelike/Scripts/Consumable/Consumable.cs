using Godot;

[GlobalClass]
public partial class Consumable : Resource
{
    public bool isUsable = true;

	[Export] public string name;
	[Export] public string description;
    [Export] public Texture2D sprite;
    [Export] public int price;
	[Export] public float value;

    [Signal] public delegate void ConsumableUsedEventHandler();

    public Hand hand;

    public virtual void OnUse()
    {
        if (!isUsable)
        {
            GD.Print("Can't be used");
            return;
        }

        GD.Print("Consumable Used: " + name);
        EmitSignal("ConsumableUsed");
    }
}
