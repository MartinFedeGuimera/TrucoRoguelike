using Godot;
using System.Collections.Generic;

public partial class ConsumableController : Node
{
	private ConsumableView view;
	private Consumable data;

	private Button sellButton;

	private DescriptionController descriptionController;

	public void SetUp(Consumable data, Hand hand, DescriptionController descriptionController)
	{
		GD.Print("Set Up called");

		view = GetNode<ConsumableView>("TextureRect");
		sellButton = GetNode<Button>("SellButton");

		view.SetUp(data.sprite, data.name);

        data.hand = hand;
        this.data = data;

		this.descriptionController = descriptionController;

		data.ConsumableUsed += OnUsed;
	}

	public void SetUp(Consumable data, DescriptionController descriptionController)
	{
        GD.Print("Set Up called");

        view = GetNode<ConsumableView>("TextureRect");
        sellButton = GetNode<Button>("SellButton");

        view.SetUp(data.sprite, data.name);

		data.isUsable = false;
		this.data = data;

        this.descriptionController = descriptionController;

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

        descriptionController.ChangeData(data.name, data.description, data.GetVarsDictionary());
        descriptionController.OnShow();
	}

	private void OnMouseExited()
	{
		sellButton.Visible = false;
		descriptionController.OnHide();
	}

    public override void _ExitTree()
    {
        data.ConsumableUsed -= OnUsed;
    }
}
