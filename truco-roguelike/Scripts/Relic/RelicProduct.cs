using Godot;

public partial class RelicProduct : Control
{
    private ShopController shop;

    private TextureRect textureRect;
    private Label priceLabel;

	private RelicController relicData;

	private DescriptionController descriptionController;

    public void SetUp(RelicController relicData, ShopController shop, DescriptionController descriptionController)
	{
		textureRect = GetNode<TextureRect>("TextureRect");
		priceLabel = GetNode<Label>("PriceLabel");

		textureRect.Texture = relicData.spriteTexture;
		priceLabel.Text = "$" + relicData.price;

		this.relicData = relicData;

		this.descriptionController = descriptionController;

		this.shop = shop;
	}

	private void OnMouseEntered()
	{
        descriptionController.ChangeData(relicData.name, relicData.description);
        descriptionController.OnShow();
	}
	private void OnMouseExited()
	{
		descriptionController.OnHide();
	}

	public void OnTryBuy()
	{
		GD.Print("Trying Buy Relic: " + relicData.name);
		shop.TryBuyRelic(relicData);
	}

	public RelicController GetData() => relicData;
}
