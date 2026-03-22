using Godot;
using System;

public enum CardSuit
{
	None,
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
	[Export] public int envidoValue;
	[Export] public int mult = 1;
	[Export] public CardSuit suit;
	[Export] public Texture2D texture;
}
