using Godot;

public partial class GameOverController : Control
{
	[Export] private Label roundLabel;
	[Export] private Label moneyLabel;

	public override void _Ready()
	{
		roundLabel.Text = "Round: " + GameData.Instance.round;
		moneyLabel.Text = "Money: $" + PlayerData.Instance.money;
	}

	private void OnQuit()
	{
        GetTree().Quit();
    }

	private void OnRestart()
	{
		PlayerData.Instance.RestartData();
		GameData.Instance.RestartData();

        SceneManager.Instance.Load("res://Scenes/Screens/Game.tscn");
    }
}
