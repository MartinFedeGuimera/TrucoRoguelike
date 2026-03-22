using Godot;

[GlobalClass]
public partial class Consumable : Resource
{
	[Export] public string name;
	[Export] public string description;
    [Export] public Texture2D sprite;
    [Export] public int price;
	[Export] public float value;

    public Hand hand;

    public virtual void OnUse()
    {
        GD.Print("Consumable Used: " + name);
    }
}
