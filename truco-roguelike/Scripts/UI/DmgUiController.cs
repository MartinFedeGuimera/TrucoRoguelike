using Godot;
using System;

public partial class DmgUiController : Node
{
	[Export] private GameController gameController;

    private Label attackLabel;
    private Label multLabel;

    float displayedAttack;
    float displayedMult;

    float targetAttack;
    float targetMult;

    public override void _Ready()
    {
        attackLabel = GetNode<Label>("AttackLabel");
        multLabel = GetNode<Label>("MultLabel");

        displayedAttack = 0;
        displayedMult = 0;
        targetAttack = 0;
        targetMult = 0;
    }

    public override void _Process(double delta)
    {
        displayedAttack = Mathf.Lerp(displayedAttack, targetAttack, 10f * (float)delta);
        displayedMult = Mathf.Lerp(displayedMult, targetMult, 10f * (float)delta);

        attackLabel.Text = $"Attack: {Mathf.RoundToInt(displayedAttack)}";
        multLabel.Text = $"Mult: {Mathf.RoundToInt(displayedMult)}";
    }

    public void UpdateUI(CardController card, float generalMult)
    {
        Card cardData = card.GetData();

        targetAttack = cardData.value;
        targetMult = cardData.mult + generalMult;
    }
}
