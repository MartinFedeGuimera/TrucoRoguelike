using Godot;
using Godot.Collections;

public partial class PlayerData : Node
{
    public static PlayerData Instance { get; private set; }

    public int money = 0;

	public Array<RelicController> relics = new Array<RelicController>();
	public int maxRelics = 4;

	public Array<Consumable> consumables = new Array<Consumable>();
    public int maxConsumables = 2;

    public int maxHealth = 100;
	public int health;

    public float permanentMult;

    [Signal]
    public delegate void DataChangedEventHandler();

    public override void _Ready()
    {
		Instance = this;
		health = maxHealth;
    }

	public void AddMoney(int addedMoney)
	{
		money += addedMoney;

		EmitSignal("DataChanged");
	}

    public void AddPermaMult(float addedPermaMult)
    {
        permanentMult += addedPermaMult;
    }

	public void RemoveRelic(string relicName)
	{
        for (int i = 0; i < relics.Count; i++)
        {
            if (relicName == relics[i].name)
            {
                relics.RemoveAt(i);
            }
        }

        EmitSignal("DataChanged");
    }

    public void RemoveConsumable(string consumableName)
    {
        for (int i = 0; i < consumables.Count; i++)
        {
            if (consumableName == consumables[i].name)
            {
                consumables.RemoveAt(i);
            }
        }

        EmitSignal("DataChanged");
    }
}
