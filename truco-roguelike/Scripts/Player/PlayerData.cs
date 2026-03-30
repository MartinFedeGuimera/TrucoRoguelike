using Godot;
using Godot.Collections;

[GlobalClass]
public partial class PlayerData : Resource
{
	public int money = 0;
	public Array<RelicController> relics;
	public Array<Consumable> consumables;
	[Export] public int maxHealth;
	public int health;

	public void Save(int money, Array<RelicController> relics, Array<Consumable> consumables, int maxHealth, int health)
	{
		this.money = money;
		this.relics = relics;
		this.consumables = consumables;
		this.maxHealth = maxHealth;
		this.health = health;
	}
}
