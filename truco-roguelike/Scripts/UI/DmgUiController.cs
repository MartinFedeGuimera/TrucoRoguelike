using Godot;
using System;

public partial class DmgUiController : Node
{
    [Export] private GameController gameController;

    [Export] private AudioStream multSound;
    private AudioStreamPlayer2D sfxPlayer;

    private Label attackLabel;
    private Label multLabel;

    float displayedAttack;
    float displayedMult;

    float targetAttack;
    float targetMult;

    float multSoundPitch = 0.8f;
    int lastPlayedMult = 0;

    public override void _Ready()
    {
        attackLabel = GetNode<Label>("AttackLabel");
        multLabel = GetNode<Label>("MultLabel");

        sfxPlayer = GetNode<AudioStreamPlayer2D>("SfxPlayer");

        displayedAttack = 0;
        displayedMult = 0;
        targetAttack = 0;
        targetMult = 0;
    }

    public override void _Process(double delta)
    {
        displayedAttack = Mathf.Lerp(displayedAttack, targetAttack, 10f * (float)delta);
        displayedMult = Mathf.Lerp(displayedMult, targetMult, 10f * (float)delta);

        int roundedAttack = Mathf.RoundToInt(displayedAttack);
        int roundedMult = Mathf.RoundToInt(displayedMult);

        attackLabel.Text = $"Attack: {roundedAttack}";
        multLabel.Text = $"Mult: {roundedMult}";

        if (roundedMult > lastPlayedMult && targetMult > 1)
        {
            PlayMultSound();
            lastPlayedMult = roundedMult;
        }
    }

    private void PlayMultSound()
    {
        if (multSound == null)
            return;

        sfxPlayer.Stream = multSound;
        sfxPlayer.PitchScale = multSoundPitch;

        multSoundPitch += 0.05f;

        sfxPlayer.Play();
    }

    public void UpdateUI(CardController card, float generalMult)
    {
        Card cardData = card.GetData();

        targetAttack = cardData.value;
        targetMult = cardData.mult + generalMult;

        multSoundPitch = 0.8f;
        lastPlayedMult = 0;
    }
}