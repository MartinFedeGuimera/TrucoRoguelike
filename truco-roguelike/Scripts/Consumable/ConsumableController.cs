using Godot;

public partial class ConsumableController : Node
{
	private ConsumableView view;
	private Consumable data;

	private Button sellButton;

	public void SetUp(Consumable data, Hand hand)
	{
		GD.Print("Set Up called");

		view = GetNode<ConsumableView>("TextureRect");
		sellButton = GetNode<Button>("SellButton");

		view.SetUp(data.sprite, data.name);

        data.hand = hand;
        this.data = data;

		data.ConsumableUsed += OnUsed;
	}

	public void SetUp(Consumable data)
	{
        GD.Print("Set Up called");

        view = GetNode<ConsumableView>("TextureRect");
        sellButton = GetNode<Button>("SellButton");

        view.SetUp(data.sprite, data.name);

		data.isUsable = false;
		this.data = data;

        data.ConsumableUsed += OnUsed;
    }

	public Consumable GetData() => data;

	public void OnButtonPressed()
	{
		GD.Print("Consumable used");
		data.OnUse();
    }

	private void OnUsed()
	{
		GD.Print("Consumable Destroyed");

        PlayerData.Instance.RemoveConsumable(data.name);
        QueueFree();
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

    public override void _ExitTree()
    {
        data.ConsumableUsed -= OnUsed;
    }
}
