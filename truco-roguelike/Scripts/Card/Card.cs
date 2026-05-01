using Godot;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
