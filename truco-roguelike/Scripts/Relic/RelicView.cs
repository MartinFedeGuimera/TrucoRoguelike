using Godot;
using System;

public partial class RelicView : Node
{
	private TextureRect textureRect;

    public void SetUp(RelicController data)
    {
        textureRect = GetNode<TextureRect>("TextureRect");

        textureRect.Texture = data.spriteTexture;
    }
}
