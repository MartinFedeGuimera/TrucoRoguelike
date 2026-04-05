using Godot;
using Godot.Collections;

[GlobalClass]
public partial class PlayerData : Resource
{
	public int money = 0;

	public Array<RelicController> relics;
	public int maxRelics;

	public Array<Consumable> consumables;
    public int maxConsumables;

    [Export] public int maxHealth;
	public int health;

	public void Save(int money, Array<RelicController> relics, int maxRelics, Array<Consumable> consumables, int maxConsumables, int maxHealth, int health)
	{
		this.money = money;
		this.relics = relics;
		this.maxRelics = maxRelics;
		this.consumables = consumables;
		this.maxConsumables = maxConsumables;
		this.maxHealth = maxHealth;
		this.health = health;
	}
}
