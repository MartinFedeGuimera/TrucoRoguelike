using Godot;
using System;

[GlobalClass]
public partial class RelicController : Resource
{
	[Export] public string name;
	[Export] public string description;
	[Export] public Texture2D spriteTexture;
	[Export] public int value;

	public Hand playerHand;

	public virtual void OnCardPlayed(Card card){}

	public virtual void OnPlayerTurnFinished(){}

	public virtual void OnEnemyAttacks(){}

	public void SetUp(Hand hand)
	{
		playerHand = hand;
	}
}
