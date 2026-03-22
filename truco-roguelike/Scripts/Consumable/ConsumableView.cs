using Godot;
using System;

public partial class ConsumableView : TextureRect
{
	[Export] private Button button;

	public void SetUp(Texture2D sprite, string name)
	{
		Texture = sprite;
		button.Text = name;
	}
}
