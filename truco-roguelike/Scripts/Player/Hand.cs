using Godot;
using Godot.Collections;

public partial class Hand : Node
{
    [Export] private GameController gameController;
    private RandomNumberGenerator seed;

    private PlayerController player;

    [Export] private PackedScene cardScene;

	private int maxCardsDrawn = 3;

	private Array<CardController> drawnCards = new Array<CardController>();

	[Export] private Deck deckResource;

    private CardController selectedCard;

    public bool canStart = false;

    private int generalMult = 0;
    private int permanentMult = 0;
    private int damageMultiplier = 0;

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

        seed = gameController.GetSeed();

        deckResource.Shuffle(seed);

        DrawCards();
    }

    public override void _Process(double delta)
    {
        if(canStart)
        {
            canStart = false;

            generalMult = permanentMult;

            DrawCards();
        }
    }

    private void DrawCards()
    {
        for(int i = 0; i < maxCardsDrawn; i++)
        {
            Card newData = deckResource.GetCards()[0];

            CardController newCard = cardScene.Instantiate<CardController>();

            newCard.SetUp(newData, this);

            drawnCards.Add(newCard);

            AddChild(newCard);

            newCard.Position = new Vector2(600 + 100 * (i + 1), 700);

            deckResource.RemoveAt(0);

            GD.Print("Drawn Card: " + newData.name);

            hasFlor = CheckFlor();
        }
    }

    public void OnCardPlayed()
    {
        if (selectedCard != null)
        {
            for (int i = 0; i < drawnCards.Count; i++)
            {
                Card card = drawnCards[i].GetData();

                Card selectedCardData = selectedCard.GetData();

                if (selectedCardData.name == card.name)
                {
                    if (player.relics != null)
                    {
                        GD.Print("Doing Relics Effects");

                        foreach (RelicController relic in player.relics)
                        {
                            relic.SetUp(this);
                            relic.OnCardPlayed(selectedCardData);
                        }
                    }
                    else
                        GD.Print("Relics Are NULL");

                    DealDamage(selectedCardData.value, selectedCardData.mult);

                    selectedCard = null;

                    drawnCards[i].QueueFree();

                    drawnCards.RemoveAt(i);

                    break;
                }
            }

            if (drawnCards.Count <= 0)
            {
                foreach (RelicController relic in player.relics)
                {
                    relic.OnPlayerTurnFinished();
                }

                EmitSignal("OutOfCards");
            }
        }
    }

    public void DealDamage(int damage, int mult)
    {
        int finalDamage = damage * (mult + generalMult);

        if(damageMultiplier > 0)
        {
            finalDamage *= damageMultiplier;
        }

        EmitSignal("Attack", finalDamage);
    }

    private bool CheckFlor()
    {
        foreach(CardController card in drawnCards)
        {
            CardSuit firstCardSuit = drawnCards[0].GetData().suit;

            for(int i = 1; i < drawnCards.Count; i++)
            {
                if(firstCardSuit != drawnCards[i].GetData().suit)
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

        EmitSignal("CardSelected", selectedCard);
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