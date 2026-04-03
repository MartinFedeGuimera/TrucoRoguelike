using Godot;
using Godot.Collections;
using System.Diagnostics;

[GlobalClass]
public partial class Deck : Resource
{
    [Export] public Array<Card> deckData;
    private Array<Card> cards;

    public void Shuffle(RandomNumberGenerator seed)
    {
        cards = deckData;

        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = seed.RandiRange(0, i);

            Card card = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = card;
        }
    }

    public Array<Card> GetCards() { return cards; }

    public void RemoveAt(int index)
    {
        cards.RemoveAt(index);
    }
}
