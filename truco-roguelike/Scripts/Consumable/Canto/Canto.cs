using Godot;

[GlobalClass]
public partial class Canto : Consumable
{
    [Export] public float valueUpgrade;
    public int level = 0;
    [Export] public int maxLevel;

    public virtual void OnLevelUp()
    {
        if(level + 1 <= maxLevel)
        {
            level++;
            value += valueUpgrade;

            GD.Print(name + " Level Up! Current Level: " + level);
            GD.Print("Value Added: " + valueUpgrade);
        }
    }

    public override void OnUse()
    {
        base.OnUse();

        OnLevelUp();
    }

    public string GetDescription()
    {
        return description;
    }
}
