using Godot;

[GlobalClass]
public partial class BilleteraMataGalan : RelicController
{
    public override void OnPlayerTurnStarted()
    {
        playerHand.AddGeneralMult(PlayerData.Instance.money);
    }
}
