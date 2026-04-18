using Godot;

public partial class RelicView : Node
{
	private TextureRect textureRect;
    private Button sellButton;
    private RelicController data;

    public void SetUp(RelicController data)
    {
        textureRect = GetNode<TextureRect>("TextureRect");
        sellButton = GetNode<Button>("SellButton");

        this.data = data;

        textureRect.Texture = data.spriteTexture;
    }

    private void OnSell()
    {
        PlayerData.Instance.AddMoney(data.price/2);
        GD.Print("Relic Sold");

        PlayerData.Instance.RemoveRelic(data.name);
        QueueFree();
    }

    private void OnMouseEntered()
    {
        sellButton.Visible = true;
    }

    private void OnMouseExited()
    {
        sellButton.Visible = false;
    }
}
