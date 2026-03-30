using Godot;

public partial class RelicProduct : Control
{
    private ShopController shop;

    private TextureRect textureRect;
    private Label priceLabel;

	private RelicController relicData;

    public void SetUp(RelicController relicData, ShopController shop)
	{
		textureRect = GetNode<TextureRect>("TextureRect");
		priceLabel = GetNode<Label>("PriceLabel");

		textureRect.Texture = relicData.spriteTexture;
		priceLabel.Text = "$" + relicData.price;

		this.relicData = relicData;

		this.shop = shop;
	}

	public void OnTryBuy()
	{
		GD.Print("Trying Buy Relic: " + relicData.name);
		shop.TryBuyRelic(relicData);
	}

	public RelicController GetData() => relicData;
}
