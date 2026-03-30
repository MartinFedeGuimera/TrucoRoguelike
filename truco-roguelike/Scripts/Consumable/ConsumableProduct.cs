using Godot;

public partial class ConsumableProduct : Control
{
    private ShopController shop;

    private Label nameLabel;
    private Label priceLabel;

    private Consumable consumableData;

    public void SetUp(Consumable consumable, ShopController shop)
    {
        nameLabel = GetNode<Label>("NameLabel");
        priceLabel = GetNode<Label>("PriceLabel");

        nameLabel.Text = consumable.name;
        priceLabel.Text = "$" + consumable.price;

        consumableData = consumable;

        this.shop = shop;
    }

    public void OnTryBuy()
    {
        GD.Print("Trying Buy Consumable: " + consumableData.name);
        shop.TryBuyConsumable(consumableData);
    }

    public Consumable GetData() => consumableData;
}
