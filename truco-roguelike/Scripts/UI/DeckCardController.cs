using Godot;

public partial class DeckCardController : Control
{
	private TextureRect textureRect;
    private DescriptionController descriptionController;

	private Card data;

    public void SetUp(Card card, DescriptionController descriptionController)
	{
		textureRect = GetNode<TextureRect>("TextureRect");
		textureRect.Texture = card.texture;

		this.descriptionController = descriptionController;
		data = card;
	}

	private void OnMouseEntered()
	{
        descriptionController.OnShow();
		descriptionController.ChangeData(data.name, data.description, data.GetVarsDictionary());
    }

	private void OnMouseExited()
	{
		descriptionController.OnHide();
	}
}
