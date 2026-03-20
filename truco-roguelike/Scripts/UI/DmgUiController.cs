using Godot;
using System;

public partial class DmgUiController : Node
{
	[Export] private GameController gameController;

    private Label attackLabel;
    private Label multLabel;

    public override void _Ready()
    {
        attackLabel = GetNode<Label>("AttackLabel");
        multLabel = GetNode<Label>("MultLabel");

        gameController.CardSelected += UpdateUI;
    }

    private void UpdateUI(CardController card)
    {
        Card cardData = card.GetData();

        attackLabel.Text = "Attack: " + cardData.value;
        multLabel.Text = "Mult: " + cardData.mult;
    }
}
