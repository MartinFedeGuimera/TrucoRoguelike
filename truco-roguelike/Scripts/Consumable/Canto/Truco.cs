using Godot;

[GlobalClass]
public partial class Truco : Canto
{
    public override void OnUse()
    {
        base.OnUse();

        hand.AddDamageMultiplier((int)value);
    }
}
