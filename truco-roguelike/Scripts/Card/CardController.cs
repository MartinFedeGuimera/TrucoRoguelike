using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

public partial class CardController : Node2D
{
    private Sprite2D sprite;

    private Card cardData;

    private PlayerController player;

    [Export] [Range(1f, 2f)] private float hooverSizeMult;

    [Export] private Vector2 cardSize = new Vector2(5f, 5f);

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D");

        player = GetParent<PlayerController>();
    }

    private void OnMouseEntered()
    {
        if(player.GetSelectedCard != null && cardData != player.GetSelectedCard())
        {
            Scale = cardSize * hooverSizeMult;
        }
    }

    private void OnMouseExited()
    {
        if (player.GetSelectedCard != null && cardData != player.GetSelectedCard())
        {
            Scale = cardSize;
        }
    }

    private void OnClicked(Node viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            player.SetSelectedCard(cardData, this);
        }
    }

    public void SetCardData(Card data)
    {
        cardData = data;

        SetUp();
    }

    private void SetUp()
    {
        sprite.SetTexture(cardData.texture);
    }
}
