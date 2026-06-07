using Godot;
using System.Collections.Generic;

[GlobalClass]
public partial class ACaraDePerro : RelicController
{
    private int previousHealth;

    [Export] public int substractValue;
    private int multAdded = 0;

    public override void OnPlayerTurnStarted()
    {
        base.OnPlayerTurnStarted();

        if (PlayerData.Instance.health < previousHealth)
        {
            playerHand.AddPermaMult(value);

            multAdded += value;
        }

        previousHealth = PlayerData.Instance.health;
    }

    public override void OnPlayerTurnFinished()
    {
        base.OnPlayerTurnFinished();


        GD.Print("Can Use");

        if (PlayerData.Instance.health > previousHealth)
        {
            playerHand.AddPermaMult(-substractValue);
            multAdded -= substractValue;

            if(multAdded < 0)
                multAdded = 0;

            GD.Print("Player healed, -" + substractValue + " perma mult");
        }
    }

    public override void OnSell()
    {
        playerHand.AddPermaMult(-multAdded);
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
