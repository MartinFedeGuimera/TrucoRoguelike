using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class ACaraDePerro : RelicController
{
    private int previousHealth;

    [Export] public int substractValue;


    public override void OnPlayerTurnStarted()
    {
        base.OnPlayerTurnStarted();

        previousHealth = PlayerData.Instance.health;
    }

    public override void OnPlayerTurnFinished()
    {
        base.OnPlayerTurnFinished();


        GD.Print("Can Use");

        if (PlayerData.Instance.health > previousHealth)
        {
            playerHand.AddPermaMult(-substractValue);

            GD.Print("Player healed, -10 perma mult");
        }
        else
        {
            playerHand.AddPermaMult(value);
        }
    }

    public override Dictionary<string, object> GetVarsDictionary()
    {
        var vars = new Dictionary<string, object>()
        {
            {"value", value},
            {"substractValue", substractValue}
        };

        return vars;
    }
}
