using Godot;

public partial class RelicView : Node
{
	private TextureRect textureRect;
    private Button sellButton;
    private RelicController data;

    private DescriptionController descriptionController;

    public void SetUp(RelicController data, DescriptionController descriptionController)
    {
        textureRect = GetNode<TextureRect>("TextureRect");
        sellButton = GetNode<Button>("SellButton");

        this.data = data;

        textureRect.Texture = data.spriteTexture;

        this.descriptionController = descriptionController;
    }

    private void OnSell()
    {
        data.OnSell();

        PlayerData.Instance.AddMoney(data.price/2);
        GD.Print("Relic Sold");

        PlayerData.Instance.RemoveRelic(data.name);
        QueueFree();
    }

    private void OnMouseEntered()
    {
        sellButton.Visible = true;

        descriptionController.ChangeData(data.name, data.description, data.GetVarsDictionary());
        descriptionController.OnShow();
    }

    private void OnMouseExited()
    {
        sellButton.Visible = false;

        descriptionController.OnHide();
    }
}
