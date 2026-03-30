using Godot;

public partial class SceneManager : Node
{
	public static SceneManager Instance;

    public override void _Ready()
    {
        Instance = this;
    }

    public void Load(string scenePath)
	{		
		GetTree().ChangeSceneToFile(scenePath);
	}
}
