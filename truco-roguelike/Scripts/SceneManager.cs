using Godot;

public partial class SceneManager : Node
{
	public void LoadSceneFile(string scenePath)
	{
		GetTree().ChangeSceneToFile(scenePath);
	}

	public void LoadPackedScene(PackedScene scene)
	{
		GetTree().ChangeSceneToPacked(scene);
	}
}
