using Godot;

[GlobalClass]
public partial class Truco : Canto
{
    public override void OnUse()
    {
        isUsable = true;

        base.OnUse();

        hand.AddGeneralMult((int)value);
    }
}
