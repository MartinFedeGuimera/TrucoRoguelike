using Godot;
using Godot.Collections;
using System.Diagnostics;

[GlobalClass]
public partial class Deck : Resource
{
    [Export] public Array<Card> cards;

    public void Shuffle(RandomNumberGenerator seed)
    {
        Array<Card> shuffledDeck = new Array<Card>();

        int deckSize = cards.Count - 1;

        for (int i = 0; i < deckSize; i++)
        {
            int randomIndex = seed.RandiRange(0, cards.Count -1);

            shuffledDeck.Add(cards[randomIndex]);
            cards.RemoveAt(randomIndex);

            deckSize--;
        }

        cards = shuffledDeck;

        foreach (Card card in cards)
        {
            Debug.WriteLine(card.name);
        }
    }

    public Array<Card> GetCards() { return cards; }

    public void RemoveAt(int index)
    {
        cards.RemoveAt(index);
    }
}
