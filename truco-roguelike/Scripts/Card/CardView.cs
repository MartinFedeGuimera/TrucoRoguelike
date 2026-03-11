using Godot;
using System;

public partial class CardView : Node2D
{
	[Export] public Card cardData;

	private Sprite2D sprite;

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D");
        sprite.Texture = cardData.texture;
    }
}
