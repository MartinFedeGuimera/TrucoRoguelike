using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class Canto : Consumable
{
    [Export] public int valueUpgrade;
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

    public override bool OnUse()
    {
        if (!base.OnUse())
            return false;

        OnLevelUp();

        return true;
    }

    public override Dictionary<string, object> GetVarsDictionary()
    {
        var vars = new Dictionary<string, object>()
        {
            {"value", value},
            {"valueUpgrade", valueUpgrade},
            {"level", level}
        };

        return vars;
    }
}
