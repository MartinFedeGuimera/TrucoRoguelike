using Godot;

[GlobalClass]
public partial class Guiso : Consumable
{
    public override void OnUse()
    {
        if(PlayerData.Instance.health == PlayerData.Instance.maxHealth)
        {
            isUsable = false;
        }
        else
        {
            isUsable = true;
        }

        base.OnUse();

        PlayerData.Instance.AddHealth((int)value);
    }
}
