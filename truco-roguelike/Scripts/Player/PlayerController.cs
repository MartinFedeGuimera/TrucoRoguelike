using Godot;
using System;
using Godot.Collections;
using System.Diagnostics;

public partial class PlayerController : Node
{
	[Export] PackedScene cardScene;

	[Export] private Array<Card> deck;
	[Export] private int maxDrawnCards = 3;

	private Array<Card> drawnCards = new Array<Card>();

    private Card selectedCard;
    private Node2D selectedCardNode;

	public override void _Ready()
	{
		DeckShuffle();

        foreach (Card card in deck)
        {
            Debug.WriteLine(card.name);
        }

		DrawCards();
    }

    public void SetSelectedCard(Card card, Node2D node)
    {
        if(selectedCardNode != null)
        {
            selectedCardNode.Scale = new Vector2(5f, 5f);
        }

        selectedCard = card;
        selectedCardNode = node;

        Debug.WriteLine("Selected Card: " + selectedCard.name);
    }

	private void DeckShuffle()
	{
		Array<Card> shuffledDeck = new Array<Card>();
		
		var rng = new RandomNumberGenerator();

        rng.Randomize();

		while(deck.Count > 0)
		{
            int ranIndex = rng.RandiRange(0, deck.Count - 1);
            shuffledDeck.Add(deck[ranIndex]);
            deck.RemoveAt(ranIndex);
        }

		deck = shuffledDeck;
	}

    private void DrawCards()
    {
        for (int i = 0; i < maxDrawnCards; i++)
        {
            if (deck.Count == 0)
                return;

            Card card = deck[0];

            drawnCards.Add(card);

            Debug.WriteLine("Drawn Card: " + card.name);

            CardController cardController = cardScene.Instantiate<CardController>();

            AddChild(cardController);

            cardController.Scale = new Vector2(5, 5);

            cardController.Position = new Vector2(450 + i * 100, 500);

            cardController.SetCardData(card);

            deck.RemoveAt(0);
        }
    }

    public Card GetSelectedCard() => selectedCard;
}
