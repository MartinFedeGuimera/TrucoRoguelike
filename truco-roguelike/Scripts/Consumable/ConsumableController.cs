using Godot;
using System;

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

	public void OnButtonPressed()
	{
		data.OnUse();
	}
}
