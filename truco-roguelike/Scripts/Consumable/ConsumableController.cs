using Godot;

public partial class ConsumableController : Node
{
	private ConsumableView view;
	private Consumable data;

	private Button sellButton;

	public void SetUp(Consumable data, Hand hand)
	{
		view = GetNode<ConsumableView>("TextureRect");
		sellButton = GetNode<Button>("SellButton");

		view.SetUp(data.sprite, data.name);

        data.hand = hand;
        this.data = data;
	}

	public void SetUp(Consumable data)
	{
		view = GetNode<ConsumableView>("TextureRect");
        sellButton = GetNode<Button>("SellButton");

        view.SetUp(data.sprite, data.name);

		data.isUsable = false;
		this.data = data;
	}

	public Consumable GetData() => data;

	public void OnButtonPressed()
	{
		data.OnUse();
	}

	private void OnSell()
	{
		PlayerData.Instance.AddMoney(data.price / 2);
		PlayerData.Instance.RemoveConsumable(data.name);

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
