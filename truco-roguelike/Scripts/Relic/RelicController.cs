using Godot;
using System;

[GlobalClass]
public partial class RelicController : Resource
{
	[Export] public string name;
	[Export] public string description;
	[Export] public Texture2D spriteTexture;
	[Export] public int value;
	[Export] public int price = 5;
	public bool wasUsed = false;

	public Hand playerHand;

	public virtual void OnCardPlayed(Card card){}

	public virtual void OnPlayerTurnFinished(){}

	public virtual void OnEnemyAttacks(){}

	public void SetUp(Hand hand)
	{
		playerHand = hand;
		wasUsed = false;
	}
}
