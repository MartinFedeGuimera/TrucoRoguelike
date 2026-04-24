using Godot;
using Godot.Collections;

public partial class Hand : Node
{
    [ExportGroup("Sounds")]
    [Export] private AudioStream drawCardSound;
    [Export] private AudioStream playCardSound;
    private AudioStreamPlayer2D sfxPlayer;

    [ExportGroup("Objects")]
    [Export] private GameController gameController;

    [Export] private Enemy enemy;
    [Export] private DmgUiController dmgUiController;

    private PlayerController player;
    private RandomNumberGenerator seed;

    [ExportGroup("Data")]
    [Export] private PackedScene cardScene;
    [Export] private Deck deckResource;

    private int maxCardsDrawn = 3;

    private Array<CardController> drawnCards = new();

    private CardController selectedCard;

    public bool canStart = true;

    private float tempMult = 0;
    private float generalMult = 0;
    private float permanentMult = 0;
    private float damageMultiplier = 1f;

    public bool hasFlor = false;

    [Signal] public delegate void CardSelectedEventHandler(CardController card);
    [Signal] public delegate void AttackEventHandler(int damage);
    [Signal] public delegate void OutOfCardsEventHandler();

    private struct AttackData
    {
        public int damage;
        public float cardMult;
        public float generalMult;
        public float tempMult;
        public float damageMultiplier;
        public CardController card;
    }

    public void CreateExternalAttack(int damage, float mult = 1f, float extraTempMult = 0f)
    {
        AttackData attack = new AttackData
        {
            damage = damage,
            cardMult = mult,
            generalMult = generalMult,
            tempMult = tempMult + extraTempMult,
            damageMultiplier = damageMultiplier,
            card = null
        };

        damageMultiplier = 1f;

        WaitForDamageAnimations(attack);
    }

    public override void _Ready()
    {
        player = GetParent<PlayerController>();
        sfxPlayer = GetNode<AudioStreamPlayer2D>("SfxPlayer");

        seed = gameController.GetSeed();
        deckResource.Shuffle(seed);
    }

    public override void _Process(double delta)
    {
        if (!canStart) return;

        canStart = false;

        generalMult = permanentMult;

        if (PlayerData.Instance.relics != null)
        {
            foreach (RelicController relic in PlayerData.Instance.relics)
            {
                relic.SetUp(this);
                relic.OnPlayerTurnStarted();
            }
        }

        DrawCards();
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

            await ToSignal(GetTree().CreateTimer(0.15f),
                SceneTreeTimer.SignalName.Timeout);
        }
    }

    public void OnCardPlayed()
    {
        if (selectedCard == null)
            return;

        tempMult = 0;

        CardController playedCard = selectedCard;
        Card cardData = playedCard.GetData();

        if (PlayerData.Instance.relics != null)
        {
            foreach (RelicController relic in PlayerData.Instance.relics)
            {
                relic.SetUp(this);
                relic.OnCardPlayed(cardData);
            }
        }

        AttackData attack = new AttackData
        {
            damage = cardData.value,
            cardMult = cardData.mult,
            generalMult = generalMult,
            tempMult = tempMult,
            damageMultiplier = damageMultiplier,
            card = playedCard
        };

        damageMultiplier = 1f;

        drawnCards.Remove(playedCard);
        playedCard.QueueFree();
        selectedCard = null;

        WaitForDamageAnimations(attack);

        if (drawnCards.Count == 0)
        {
            if (PlayerData.Instance.relics != null)
            {
                foreach (RelicController relic in PlayerData.Instance.relics)
                    relic.OnPlayerTurnFinished();
            }

            EmitSignal("OutOfCards");
        }
    }

    private async void WaitForDamageAnimations(AttackData attack)
    {
        dmgUiController.UpdateUI(
            attack.card,
            attack.generalMult,
            attack.tempMult);

        sfxPlayer.Stream = playCardSound;
        sfxPlayer.PitchScale = seed.RandfRange(0.8f, 1.1f);
        sfxPlayer.Play();

        await ToSignal(GetTree().CreateTimer(0.3f),
            SceneTreeTimer.SignalName.Timeout);

        DealDamage(attack);
    }

    private void DealDamage(AttackData attack)
    {
        float finalDamage = attack.damage * (attack.cardMult + attack.generalMult + attack.tempMult);

        finalDamage *= attack.damageMultiplier;

        EmitSignal("Attack", (int)finalDamage);
    }

    private bool CheckFlor()
    {
        if (drawnCards.Count == 0)
            return false;

        CardSuit firstSuit = drawnCards[0].GetData().suit;

        for (int i = 1; i < drawnCards.Count; i++)
        {
            if (drawnCards[i].GetData().suit != firstSuit)
                return false;
        }

        return true;
    }

    public void SetSelectedCard(CardController card)
    {
        selectedCard = card;

        dmgUiController.UpdateUI(selectedCard, generalMult, tempMult);
        EmitSignal("CardSelected", selectedCard);
    }

    public void AddTempMult(int addedMult) => tempMult += addedMult;

    public void AddGeneralMult(int addedMult) => generalMult += addedMult;

    public void AddPermaMult(int addedMult)
    {
        permanentMult = Mathf.Max(0, permanentMult + addedMult);
    }

    public void AddDamageMultiplier(float mult)
    {
        damageMultiplier *= mult;
    }

    public Array<Card> GetDrawnCards()
    {
        Array<Card> cardsData = new();

        foreach (CardController controller in drawnCards)
            cardsData.Add(controller.GetData());

        return cardsData;
    }
}