using Godot;
using System;

public enum CardSuit
{
	Espada,
	Copa,
	Oro,
	Basto
}

[GlobalClass]
public partial class Card : Resource
{
	[Export] public string name;
	[Export] public int value;
	[Export] public CardSuit suit;
	[Export] public Texture2D texture;
}
