using Godot;
using Godot.Collections;

public partial class Hand : Node
{
    [Export] private GameController gameController;
    private RandomNumberGenerator seed;

    [Export] private PackedScene cardScene;

	private int maxCardsDrawn = 3;

	private Array<CardController> drawnCards = new Array<CardController>();

	[Export] private Deck deckResource;

    private CardController selectedCard;

    private int mult = 1;

    public bool canStart = false;

    [Signal]
    public delegate void CardSelectedEventHandler(CardController card, int mult);

    [Signal]
    public delegate void AttackEventHandler(int damage);

    [Signal]
    public delegate void OutOfCardsEventHandler();

    public override void _Ready()
    {
        seed = gameController.GetSeed();

        deckResource.Shuffle(seed);

        DrawCards();
    }

    public override void _Process(double delta)
    {
        if(canStart)
        {
            canStart = false;

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
        }
    }

    public void OnCardPlayed()
    {
        for(int i = 0; i < drawnCards.Count;  i++)
        {
            Card card = drawnCards[i].GetData();

            Card selectedCardData = selectedCard.GetData();

            if(selectedCardData.name == card.name)
            {
                DealDamage(selectedCardData.value);

                selectedCard = null;

                drawnCards[i].QueueFree();

                drawnCards.RemoveAt(i);

                break;
            }
        }

        if (drawnCards.Count <= 0)
        {
            EmitSignal("OutOfCards");
        }
    }

    private void DealDamage(int damage)
    {
        EmitSignal("Attack", damage * mult);
    }

    public void SetSelectedCard(CardController card)
    {
        selectedCard = card;

        EmitSignal("CardSelected", selectedCard, mult);
    }
}