using Godot;

public partial class ConsumableProduct : Control
{
    private ShopController shop;

    private Label nameLabel;
    private Label priceLabel;

    private Consumable consumableData;

    private DescriptionController descriptionController;

    public void SetUp(Consumable consumable, ShopController shop, DescriptionController descriptionController)
    {
        nameLabel = GetNode<Label>("NameLabel");
        priceLabel = GetNode<Label>("PriceLabel");

        nameLabel.Text = consumable.name;
        priceLabel.Text = "$" + consumable.price;

        consumableData = consumable;

        this.descriptionController = descriptionController;


        this.shop = shop;
    }

    private void OnMouseEntered()
    {
        descriptionController.ChangeData(consumableData.name, consumableData.description, consumableData.GetVarsDictionary());
        descriptionController.OnShow();
    }
    private void OnMouseExited()
    {
        descriptionController.OnHide();
    }

    public void OnTryBuy()
    {
        GD.Print("Trying Buy Consumable: " + consumableData.name);
        shop.TryBuyConsumable(consumableData);
    }

    public Consumable GetData() => consumableData;
}
