using Godot;
using Godot.Collections;

public partial class ShopController : Node
{
    [Export] private GameData gameData;
    private RandomNumberGenerator seed;

	[ExportGroup("Shop Settings")]
	[Export] private int maxRelics = 3;
	[Export] private int maxConsumables = 2;
    [Export] private int rerollPrice;

	[ExportGroup("Data Settings")]
    [Export] private PlayerData playerData;

	[Export] private Array<RelicController> relicsData;
    private Array<RelicController> relics;
    [Export] private PackedScene relicProductScene;

	[Export] private Array<Consumable> consumablesData;
	[Export] private Array<Consumable> consumables;
    [Export] private PackedScene consumableProductScene;

    [Export] private PackedScene relicScene;
    [Export] private PackedScene consumableScene;

    // General Vars
    private Array<RelicProduct> drawnRelics;
    private Array<ConsumableProduct> drawnConsumables;

    // Child Nodes
    private HBoxContainer relicsContainer;
    private VBoxContainer consumablesContainer;

    private Label moneyLabel;
    private HBoxContainer relicsProductsContainer;
    private HBoxContainer consumablesProductsContainer;

    public override void _Ready()
    {
        GD.Print(playerData.GetInstanceId());

        moneyLabel = GetNode<Label>("MoneyLabel");
        relicsProductsContainer = GetNode<HBoxContainer>("ShopContent/ProductsContainer/RelicsContainer");
        consumablesProductsContainer = GetNode<HBoxContainer>("ShopContent/ProductsContainer/ConsumablesContainer");

        relicsContainer = GetNode<HBoxContainer>("PlayerData/RelicsContainer");
        consumablesContainer = GetNode<VBoxContainer>("PlayerData/ConsumablesContainer");

        drawnRelics = new Array<RelicProduct>();
        drawnConsumables = new Array<ConsumableProduct>();

        seed = gameData.seed;

        relics = relicsData;
        consumables = consumablesData;

        UpdatePlayerDataUI();

        SetUp();
    }

    private void ClearPlayerDataUI()
    {
        if(relicsContainer.GetChildren() != null)
        {
            foreach (Node child in relicsContainer.GetChildren())
            {
                child.QueueFree();
            }
        }

        if(consumablesContainer.GetChildren() != null)
        {
            foreach (Node child in consumablesContainer.GetChildren())
            {
                child.QueueFree();
            }
        }
    }

    private void UpdatePlayerDataUI()
    {
        moneyLabel.Text = "$" + playerData.money;

        ClearPlayerDataUI();

        if (playerData.relics != null)
        {
            foreach (RelicController relic in playerData.relics)
            {
                RelicView relicView = relicScene.Instantiate<RelicView>();

                relicView.SetUp(relic);

                relicsContainer.AddChild(relicView);
            }
        }

        if (playerData.consumables != null)
        {
            foreach (Consumable consumable in playerData.consumables)
            {
                ConsumableController controller = consumableScene.Instantiate<ConsumableController>();

                controller.SetUp(consumable);

                consumablesContainer.AddChild(controller);
            }
        }
    }

    private void SetUp()
    {
        ShuffleRelics();
        DrawRelics();

        ShuffleConsumables();
        DrawConsumables();
    }

    public void OnReRoll()
    {
        if(playerData.money >= rerollPrice)
        {
            rerollPrice += 2;

            foreach (Node relic in relicsProductsContainer.GetChildren())
            {
                relic.QueueFree();
            }

            foreach(RelicProduct relic in drawnRelics)
            {
                drawnRelics.Remove(relic);
            }

            foreach (Node consumable in consumablesProductsContainer.GetChildren())
            {
                consumable.QueueFree();
            }

            foreach (ConsumableProduct consumable in drawnConsumables)
            {
                drawnConsumables.Remove(consumable);
            }

            SetUp();
        }
        else
        {
            GD.Print("Not Enough Money");
        }    
    }

    public void OnContinue()
    {
        SceneManager.Instance.Load("res://Scenes/Screens/Game.tscn");
    }

    public void TryBuyRelic(RelicController relicData)
    {
        if(playerData.money >= relicData.price)
        {
            playerData.money -= relicData.price;

            GD.Print(relicData.name + "Added to Relics");
            playerData.relics.Add(relicData);

            for (int i = drawnRelics.Count - 1; i >= 0; i--)
            {
                if (drawnRelics[i].GetData().name == relicData.name)
                {
                    drawnRelics.RemoveAt(i);
                }
            }

            for (int i = relics.Count - 1; i >= 0; i--)
            {
                if (relics[i].name == relicData.name)
                {
                    relics.RemoveAt(i);
                }
            }

            for (int i = relicsProductsContainer.GetChildren().Count - 1; i >= 0; i--)
            {
                Node child = relicsProductsContainer.GetChildren()[i];

                RelicProduct childRelic = child as RelicProduct;

                if (childRelic != null && childRelic.GetData().name == relicData.name)
                {
                    child.QueueFree();
                }
            }

            UpdatePlayerDataUI();
        }
        else
        {
            GD.Print("Not Enough Money");
        }
    }

    public void TryBuyConsumable(Consumable consumableData)
    {
        if (playerData.money >= consumableData.price)
        {
            playerData.money -= consumableData.price;

            playerData.consumables.Add(consumableData);

            for (int i = drawnConsumables.Count - 1; i >= 0; i--)
            {
                if (drawnConsumables[i].GetData().name == consumableData.name)
                {
                    drawnConsumables.RemoveAt(i);
                }
            }

            for (int i = consumables.Count - 1; i >= 0; i--)
            {
                if (consumables[i].name == consumableData.name)
                {
                    consumables.RemoveAt(i);
                }
            }

            for (int i = consumablesProductsContainer.GetChildren().Count - 1; i >= 0; i--)
            {
                Node child = consumablesProductsContainer.GetChildren()[i];

                ConsumableProduct childConsumable = child as ConsumableProduct;

                if (childConsumable != null && childConsumable.GetData().name == consumableData.name)
                {
                    child.QueueFree();
                }
            }

            UpdatePlayerDataUI();
        }
        else
        {
            GD.Print("Not Enough Money");
        }
    }

    private void ShuffleRelics()
    {
        for (int i = relics.Count - 1; i > 0; i--)
        {
            int randomIndex = seed.RandiRange(0, i);

            RelicController currentRelic = relics[i];
            relics[i] = relics[randomIndex];
            relics[randomIndex] = currentRelic;
        }
    }

    private void DrawRelics()
    {
        int amount = Mathf.Min(maxRelics, relics.Count);

        for (int i = 0; i < amount; i++)
        {
            RelicController newRelicData = relics[0];

            RelicProduct newRelic = relicProductScene.Instantiate<RelicProduct>();

            newRelic.SetUp(newRelicData, this);

            drawnRelics.Add(newRelic);

            relicsProductsContainer.AddChild(newRelic);

            relics.RemoveAt(0);
        }
    }

    private void ShuffleConsumables()
    {
        for (int i = relics.Count - 1; i > 0; i--)
        {
            int randomIndex = seed.RandiRange(0, i);

            Consumable currentConsumable = consumables[i];
            consumables[i] = consumables[randomIndex];
            consumables[randomIndex] = currentConsumable;
        }
    }

    private void DrawConsumables()
    {
        int amount = Mathf.Min(maxConsumables, consumables.Count);

        for (int i = 0; i < amount; i++)
        {
            Consumable newConsumableData = consumables[0];

            ConsumableProduct newConsumable = consumableProductScene.Instantiate<ConsumableProduct>();

            newConsumable.SetUp(newConsumableData, this);

            drawnConsumables.Add(newConsumable);

            consumablesProductsContainer.AddChild(newConsumable);

            consumables.RemoveAt(0);
        }
    }
}
