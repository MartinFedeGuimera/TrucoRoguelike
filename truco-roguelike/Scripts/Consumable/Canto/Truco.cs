using Godot;

[GlobalClass]
public partial class Truco : Canto
{
    public override bool OnUse()
    {
        if(hand != null)
        {
            isUsable = true;
        }

        if (!base.OnUse())
            return false;

        hand.AddGeneralMult((int)value);

        return true;
    }
}
