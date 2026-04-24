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
    [Export] private Array<RelicController> relicsData;
    private Array<RelicController> relics;
    [Export] private PackedScene relicProductScene;

    [Export] private Array<Consumable> consumablesData;
    private Array<Consumable> consumables;
    [Export] private PackedScene consumableProductScene;

    [Export] private PackedScene relicScene;
    [Export] private PackedScene consumableScene;

    private Array<RelicProduct> drawnRelics;
    private Array<ConsumableProduct> drawnConsumables;

    private HBoxContainer relicsContainer;
    private VBoxContainer consumablesContainer;

    private Label moneyLabel;
    private HBoxContainer relicsProductsContainer;
    private HBoxContainer consumablesProductsContainer;

    [ExportGroup("Sounds")]
    private AudioStreamPlayer2D sfxPlayer;
    [Export] private AudioStream buySound;

    RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        moneyLabel = GetNode<Label>("MoneyLabel");
        relicsProductsContainer = GetNode<HBoxContainer>("ShopContent/ProductsContainer/RelicsContainer");
        consumablesProductsContainer = GetNode<HBoxContainer>("ShopContent/ProductsContainer/ConsumablesContainer");

        relicsContainer = GetNode<HBoxContainer>("PlayerData/RelicsContainer");
        consumablesContainer = GetNode<VBoxContainer>("PlayerData/ConsumablesContainer");

        sfxPlayer = GetNode<AudioStreamPlayer2D>("SfxPlayer");

        drawnRelics = new Array<RelicProduct>();
        drawnConsumables = new Array<ConsumableProduct>();

        PlayerData.Instance.DataChanged += UpdatePlayerDataUI;

        seed = gameData.seed;
        rng.Randomize();

        relics = new Array<RelicController>();

        foreach (var relic in relicsData)
            relics.Add((RelicController)relic.Duplicate(true));

        consumables = new Array<Consumable>();

        foreach (var consumable in consumablesData)
            consumables.Add((Consumable)consumable.Duplicate(true));

        UpdatePlayerDataUI();

        SetUp();
    }

    public override void _ExitTree()
    {
        PlayerData.Instance.DataChanged -= UpdatePlayerDataUI;
    }

    private void ClearPlayerDataUI()
    {
        if (relicsContainer.GetChildren() != null)
        {
            foreach (Node child in relicsContainer.GetChildren())
                child.QueueFree();
        }

        if (consumablesContainer.GetChildren() != null)
        {
            foreach (Node child in consumablesContainer.GetChildren())
                child.QueueFree();
        }
    }

    private void UpdatePlayerDataUI()
    {
        moneyLabel.Text = "$" + PlayerData.Instance.money;

        ClearPlayerDataUI();

        if (PlayerData.Instance.relics != null)
        {
            foreach (RelicController relic in PlayerData.Instance.relics)
            {
                RelicView relicView = relicScene.Instantiate<RelicView>();
                relicView.SetUp(relic);
                relicsContainer.AddChild(relicView);
            }
        }

        if (PlayerData.Instance.consumables != null)
        {
            foreach (Consumable consumable in PlayerData.Instance.consumables)
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
        if (PlayerData.Instance.money >= rerollPrice)
        {
            rerollPrice += 2;

            sfxPlayer.PitchScale = rng.RandfRange(0.8f, 1.1f);
            sfxPlayer.Stream = buySound;
            sfxPlayer.Play();

            foreach (Node relic in relicsProductsContainer.GetChildren())
                relic.QueueFree();

            drawnRelics.Clear();

            foreach (Node consumable in consumablesProductsContainer.GetChildren())
                consumable.QueueFree();

            drawnConsumables.Clear();

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
        if (PlayerData.Instance.money >= relicData.price &&
            PlayerData.Instance.relics.Count + 1 <= PlayerData.Instance.maxRelics)
        {
            PlayerData.Instance.money -= relicData.price;

            sfxPlayer.PitchScale = rng.RandfRange(0.8f, 1.1f);
            sfxPlayer.Stream = buySound;
            sfxPlayer.Play();

            GD.Print(relicData.name + "Added to Relics");
            PlayerData.Instance.relics.Add(relicData);

            for (int i = drawnRelics.Count - 1; i >= 0; i--)
            {
                if (drawnRelics[i].GetData().name == relicData.name)
                    drawnRelics.RemoveAt(i);
            }

            for (int i = relics.Count - 1; i >= 0; i--)
            {
                if (relics[i].name == relicData.name)
                    relics.RemoveAt(i);
            }

            for (int i = relicsProductsContainer.GetChildren().Count - 1; i >= 0; i--)
            {
                Node child = relicsProductsContainer.GetChildren()[i];
                RelicProduct childRelic = child as RelicProduct;

                if (childRelic != null && childRelic.GetData().name == relicData.name)
                    child.QueueFree();
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
        if (PlayerData.Instance.money >= consumableData.price &&
            PlayerData.Instance.consumables.Count + 1 <= PlayerData.Instance.maxConsumables)
        {
            PlayerData.Instance.money -= consumableData.price;

            sfxPlayer.PitchScale = rng.RandfRange(0.8f, 1.1f);
            sfxPlayer.Stream = buySound;
            sfxPlayer.Play();

            PlayerData.Instance.consumables.Add(consumableData);

            for (int i = drawnConsumables.Count - 1; i >= 0; i--)
            {
                if (drawnConsumables[i].GetData().name == consumableData.name)
                    drawnConsumables.RemoveAt(i);
            }

            for (int i = consumables.Count - 1; i >= 0; i--)
            {
                if (consumables[i].name == consumableData.name)
                    consumables.RemoveAt(i);
            }

            for (int i = consumablesProductsContainer.GetChildren().Count - 1; i >= 0; i--)
            {
                Node child = consumablesProductsContainer.GetChildren()[i];
                ConsumableProduct childConsumable = child as ConsumableProduct;

                if (childConsumable != null &&
                    childConsumable.GetData().name == consumableData.name)
                    child.QueueFree();
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
        if (relics != null)
        {
            for (int i = relics.Count - 1; i > 0; i--)
            {
                int randomIndex = seed.RandiRange(0, i);

                RelicController currentRelic = relics[i];
                relics[i] = relics[randomIndex];
                relics[randomIndex] = currentRelic;
            }
        }
    }

    private void DrawRelics()
    {
        while (drawnRelics.Count < maxRelics && relics.Count > 0)
        {
            RelicController newRelicData = relics[0];
            relics.RemoveAt(0);

            bool isRepeated = false;

            foreach (var ownedRelic in PlayerData.Instance.relics)
            {
                if (ownedRelic.name == newRelicData.name)
                {
                    isRepeated = true;
                    break;
                }
            }

            if (isRepeated)
                continue;

            RelicProduct newRelic = relicProductScene.Instantiate<RelicProduct>();
            newRelic.SetUp(newRelicData, this);

            drawnRelics.Add(newRelic);
            relicsProductsContainer.AddChild(newRelic);
        }
    }

    private void ShuffleConsumables()
    {
        if (consumables != null)
        {
            for (int i = consumables.Count - 1; i > 0; i--)
            {
                int randomIndex = seed.RandiRange(0, i);

                Consumable currentConsumable = consumables[i];
                consumables[i] = consumables[randomIndex];
                consumables[randomIndex] = currentConsumable;
            }
        }
    }

    private void DrawConsumables()
    {
        int amount = Mathf.Min(maxConsumables, consumables.Count);

        for (int i = 0; i < amount; i++)
        {
            Consumable newConsumableData = consumables[0];

            bool isRepeated = false;

            for (int j = 0; j < PlayerData.Instance.consumables.Count; j++)
            {
                if (newConsumableData.name ==
                    PlayerData.Instance.consumables[j].name)
                {
                    isRepeated = true;
                    break;
                }
            }

            if (!isRepeated)
            {
                ConsumableProduct newConsumable = consumableProductScene.Instantiate<ConsumableProduct>();

                newConsumable.SetUp(newConsumableData, this);

                drawnConsumables.Add(newConsumable);
                consumablesProductsContainer.AddChild(newConsumable);

                consumables.RemoveAt(0);
            }
        }
    }
}