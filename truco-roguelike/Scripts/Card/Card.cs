using Godot;
using System.Collections.Generic;

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
	public string description = "Damage {value} \nMult {mult} \nEnvido Damage {envidoValue} \nSuit {suit}";

    public virtual Dictionary<string, object> GetVarsDictionary()
    {
        var vars = new Dictionary<string, object>()
        {
            {"value", value},
			{"mult", mult},
			{"envidoValue", envidoValue},
			{"suit", suit}
        };

        return vars;
    }
}
