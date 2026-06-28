using Godot;

public partial class DeckController : Control
{
    [Export] private VBoxContainer[] containers;

    [Export] private DescriptionController descriptionController;

    [Export] private Deck deckResource;
    [Export] private PackedScene deckCardScene;

    public override void _Ready()
    {
        foreach (var card in deckResource.GetCards())
        {
            DeckCardController newCard = deckCardScene.Instantiate<DeckCardController>();

            int index = 15 - card.value;

            if (index >= 0 && index < containers.Length)
            {
                containers[index].AddChild(newCard);
            }

            newCard.SetUp(card, descriptionController);
        }
    }

    private void OnDeck()
    {
        Visible = !Visible;

        GD.Print("Button Clicked!");

        if (Visible)
            RefreshDeck();
    }

    private void RefreshDeck()
    {
        foreach (var container in containers)
        {
            foreach (Node child in container.GetChildren())
            {
                child.QueueFree();
            }
        }

        foreach (var card in deckResource.GetCards())
        {
            DeckCardController newCard =
                deckCardScene.Instantiate<DeckCardController>();

            int index = 15 - card.value;

            if (index >= 0 && index < containers.Length)
            {
                containers[index].AddChild(newCard);
            }

            newCard.SetUp(card, descriptionController);
        }
    }
}
