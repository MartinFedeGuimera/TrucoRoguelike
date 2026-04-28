using Godot;
using System.Collections.Generic;

public partial class DmgUiController : Node
{
    [Export] private GameController gameController;

    [Export] private AudioStream multSound;
    private AudioStreamPlayer2D sfxPlayer;

    private Label attackLabel;
    private Label multLabel;

    int displayedAttack;
    float displayedMult;

    float lastMult;

    float multSoundPitch = 0.8f;
    int lastPlayedMult = 0;

    private Queue<float> multQueue = new();

    private bool processingQueue = false;

    public override void _Ready()
    {
        attackLabel = GetNode<Label>("AttackLabel");
        multLabel = GetNode<Label>("MultLabel");

        sfxPlayer = GetNode<AudioStreamPlayer2D>("SfxPlayer");

        displayedAttack = 0;
        displayedMult = 0;
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

    private async void ProcessQueue()
    {
        if (processingQueue)
            return;

        processingQueue = true;

        float delay = 0.2f;

        while (multQueue.Count > 0)
        {
            float nextMult = multQueue.Dequeue();

            displayedMult = nextMult;
            UpdateLabels();

            PlayMultSound();

            lastMult = displayedMult;

            await ToSignal(
                GetTree().CreateTimer(delay),
                SceneTreeTimer.SignalName.Timeout);

            delay *= 0.8f;
            delay = Mathf.Max(0.03f, delay);
        }

        multSoundPitch = 0.8f;
        processingQueue = false;
    }

    public void UpdateFromCard(CardController card, float generalMult, float tempMult)
    {
        multQueue.Clear();

        multSoundPitch = 0.8f;

        int attack = 0;
        float mult = generalMult + tempMult;

        if (card != null)
        {
            Card data = card.GetData();
            attack = data.value;
            mult += data.mult;
        }

        displayedAttack = attack;
        displayedMult = mult;
        lastMult = mult;

        UpdateLabels();
    }

    public void ApplyPerkMult(float newMult)
    {
        if (newMult <= lastMult)
            return;

        multQueue.Enqueue(newMult);

        ProcessQueue();
    }

    private void UpdateLabels()
    {
        attackLabel.Text = $"Attack: {displayedAttack}";
        multLabel.Text = $"Mult: {displayedMult}";
    }

    public void ResetRound()
    {
        multQueue.Clear();
        processingQueue = false;

        displayedAttack = 0;
        displayedMult = 0;
        lastMult = 0;

        multSoundPitch = 0.8f;

        UpdateLabels();
    }
}