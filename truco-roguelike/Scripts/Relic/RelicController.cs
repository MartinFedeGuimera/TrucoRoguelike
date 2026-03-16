using Godot;
using System;

[GlobalClass]
public partial class RelicController : Resource
{
	[Export] public string name;
	[Export] public string description;
	[Export] public Texture2D spriteTexture;
	[Export] public int value;
}
