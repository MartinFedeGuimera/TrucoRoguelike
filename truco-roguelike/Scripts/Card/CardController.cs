using Godot;
using System;
using System.Diagnostics;

public partial class CardController : Node2D
{
	private Card data;
	private Sprite2D sprite;

	[Export] private float hooverMult = 1.2f;
	[Export] private Vector2 cardSize = new Vector2(5f, 5f);

    private bool isSelected = false;

	private Hand playerHand;

    public override void _Ready()
    {
		sprite = GetNode<Sprite2D>("Sprite2D");

		playerHand.CardSelected += OnCardSelected;
    }

    private void OnCardSelected(CardController cardController, int mult)
	{
		Card cardData = cardController.GetData();

        if(cardData.name != data.name)
		{
			Scale = cardSize;
			isSelected = false;
		}
    }

	public void OnInputEvent(Node viewport, InputEvent @event, int shapeIdx)
	{
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            isSelected = true;

			playerHand.SetSelectedCard(this);

            Scale = cardSize * hooverMult;
        }
    }

	public void OnMouseEntered()
	{
		if(!isSelected)
		{
            Scale = cardSize * hooverMult;
        }
	}

	public void OnMouseExited()
	{
		if(!isSelected)
		{
            Scale = cardSize;
        }
	}

	public void SetUp(Card data, Hand hand)
	{
		this.data = data;
		playerHand = hand;

		sprite = GetNode<Sprite2D>("Sprite2D");
		sprite.Texture = this.data.texture;

		Scale = cardSize;
	}

	public Card GetData() => data;

    public override void _ExitTree()
    {
		playerHand.CardSelected -= OnCardSelected;
    }
}
