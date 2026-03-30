using Godot;

public partial class ConsumableController : Node
{
	private ConsumableView view;
	private Consumable data;

	public void SetUp(Consumable data, Hand hand)
	{
		view = GetNode<ConsumableView>("TextureRect");

		view.SetUp(data.sprite, data.name);

        data.hand = hand;
        this.data = data;
	}

	public void SetUp(Consumable data)
	{
		view = GetNode<ConsumableView>("TextureRect");

		view.SetUp(data.sprite, data.name);

		data.isUsable = false;
		this.data = data;
	}

	public Consumable GetData() => data;

	public void OnButtonPressed()
	{
		data.OnUse();
	}
}
