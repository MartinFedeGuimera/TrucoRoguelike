using Godot;

[GlobalClass]
public partial class ACaraDePerro : RelicController
{
    private bool canUse = false;
    private int previousHealth;

    [Export] public int substractValue;

    public override void OnPlayerTurnStarted()
    {
        base.OnPlayerTurnStarted();

        if(canUse)
        {
            if(PlayerData.Instance.health > previousHealth)
            {
                playerHand.AddPermaMult(-substractValue);
            }
            else
            {
                playerHand.AddPermaMult(value);
            }
        }
    }

    public override void OnPlayerTurnFinished()
    {
        base.OnPlayerTurnFinished();

        canUse = true;
        previousHealth = PlayerData.Instance.health;
    }
}
