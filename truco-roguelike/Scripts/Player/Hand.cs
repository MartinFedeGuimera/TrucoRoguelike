using Godot;
using Godot.Collections;
using System.Collections;

public partial class Hand : Node
{
    [ExportGroup("Sounds")]
    [Export] private AudioStream drawCardSound;
    [Export] private AudioStream playCardSound;
    private AudioStreamPlayer2D sfxPlayer;

    [ExportGroup("Objects")]
    [Export] private GameController gameController;
    private RandomNumberGenerator seed;

    [Export] private Enemy enemy;

    [Export] private DmgUiController dmgUiController;

    private PlayerController player;

    [ExportGroup("Data")]
    [Export] private PackedScene cardScene;
    [Export] private Deck deckResource;

    private int maxCardsDrawn = 3;

	private Array<CardController> drawnCards = new Array<CardController>();

    private CardController selectedCard;

    public bool canStart = false;

    private float tempMult = 0;
    private float generalMult = 0;
    private float permanentMult = 0;
    private float damageMultiplier = 1;

    public bool hasFlor = false;

    [Signal]
    public delegate void CardSelectedEventHandler(CardController card);

    [Signal]
    public delegate void AttackEventHandler(int damage);

    [Signal]
    public delegate void OutOfCardsEventHandler();

    public override void _Ready()
    {
        player = GetParent<PlayerController>();
        sfxPlayer = GetNode<AudioStreamPlayer2D>("SfxPlayer");

        seed = gameController.GetSeed();

        deckResource.Shuffle(seed);

        DrawCards();
    }

    public override void _Process(double delta)
    {
        if (canStart)
        {
            canStart = false;

            generalMult = permanentMult;

            DrawCards();
        }
    }

    private async void DrawCards()
    {
        for (int i = 0; i < maxCardsDrawn; i++)
        {
            if (deckResource.GetCards().Count == 0)
                return;

            Card newData = deckResource.GetCards()[0];

            CardController newCard = cardScene.Instantiate<CardController>();
            newCard.SetUp(newData, this);

            drawnCards.Add(newCard);
            AddChild(newCard);

            newCard.Position = new Vector2(600 + 100 * (i + 1), 700);

            deckResource.RemoveAt(0);

            hasFlor = CheckFlor();

            sfxPlayer.Stream = drawCardSound; 
            sfxPlayer.PitchScale = seed.RandfRange(0.8f, 1.1f);
            sfxPlayer.Play();

            await ToSignal(
                GetTree().CreateTimer(0.15f),
                SceneTreeTimer.SignalName.Timeout
            );
        }
    }

    public void OnCardPlayed()
    {
        tempMult = 0;

        if (selectedCard != null)
        {
            for (int i = 0; i < drawnCards.Count; i++)
            {
                Card card = drawnCards[i].GetData();

                Card selectedCardData = selectedCard.GetData();

                if (selectedCardData.name == card.name)
                {
                    int relicsUsed = 0;

                    if (player.relics != null)
                    {
                        GD.Print("Doing Relics Effects");

                        foreach (RelicController relic in player.relics)
                        {
                            relic.SetUp(this);
                            relic.OnCardPlayed(selectedCardData);
                            
                            if(relic.wasUsed == true)
                                relicsUsed++;
                        }
                    }

                    WaitForDamageAnimations(card.value, card.mult, relicsUsed);

                    selectedCard = null;

                    drawnCards[i].QueueFree();

                    drawnCards.RemoveAt(i);

                    break;
                }
            }

            if (drawnCards.Count <= 0)
            {
                if(player.relics != null)
                {
                    foreach (RelicController relic in player.relics)
                    {
                        relic.OnPlayerTurnFinished();
                    }
                }

                EmitSignal("OutOfCards");
            }
        }
    }

    private async void WaitForDamageAnimations(int damage, int mult, int waitTime)
    {
        dmgUiController.UpdateUI(selectedCard, generalMult);

        sfxPlayer.Stream = playCardSound; 
        sfxPlayer.PitchScale = seed.RandfRange(0.8f, 1.1f); 
        sfxPlayer.Play();

        await ToSignal(GetTree().CreateTimer(waitTime), SceneTreeTimer.SignalName.Timeout);

        DealDamage(damage, mult);
    }

    public void DealDamage(int damage, float mult)
    {
        float finalDamage = damage * (mult + generalMult + tempMult);

        if (damageMultiplier > 0)
        {
            finalDamage *= damageMultiplier;
            damageMultiplier = 1;
        }

        EmitSignal("Attack", (int)finalDamage);
    }

    private bool CheckFlor()
    {
        for(int i = 0; i < drawnCards.Count; i++)
        {
            CardSuit firstCardSuit = drawnCards[0].GetData().suit;

            for(int j = 1; j < drawnCards.Count; j++)
            {
                if(firstCardSuit != drawnCards[j].GetData().suit)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void SetSelectedCard(CardController card)
    {
        selectedCard = card;

        dmgUiController.UpdateUI(selectedCard, generalMult);

        EmitSignal("CardSelected", selectedCard);
    }

    public void AddTempMult(int addedMult)
    {
        tempMult += addedMult;
    }

    public void AddGeneralMult(int addedMult)
    {
        generalMult += addedMult;
    }

    public void AddPermaMult(int addedMult)
    {
        permanentMult += addedMult;
    }

    public void AddDamageMultiplier(int addedMultiplier)
    {
        damageMultiplier += addedMultiplier;
    }

    public Array<Card> GetDrawnCards()
    {
        Array<Card> cardsData = new Array<Card>();

        foreach(CardController controller in drawnCards)
        {
            cardsData.Add(controller.GetData());
        }

        return cardsData;
    }
}