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

    public virtual bool OnUse()
    {
        if (!isUsable)
        {
            GD.Print("Consumable can't be used");
            return false;
        }

        if (hand == null)
        {
            GD.Print("Hand is NULL");
            return false;
        }

        GD.Print("Consumable Used: " + name);
        EmitSignal("ConsumableUsed");

        return true;
    }
}
