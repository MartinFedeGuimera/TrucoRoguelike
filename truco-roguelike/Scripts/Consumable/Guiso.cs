using Godot;

[GlobalClass]
public partial class Guiso : Consumable
{
    public override bool OnUse()
    {
        if(PlayerData.Instance.health == PlayerData.Instance.maxHealth)
        {
            isUsable = false;
        }
        else
        {
            isUsable = true;
        }

        if (!base.OnUse())
            return false;

        PlayerData.Instance.AddHealth((int)value);

        return true;
    }
}
