using Godot;

public partial class DescriptionController : Control
{
    private Label title;
    private Label description;

    private bool isVisible = false;

    [Export] private float maxDescriptionWidth = 300f;

    public void SetUp()
    {
        title = GetNode<Label>("MarginContainer/VBoxContainer/Title");
        description = GetNode<Label>("MarginContainer/VBoxContainer/Description");

        description.AutowrapMode = TextServer.AutowrapMode.Word;

        description.CustomMinimumSize = new Vector2(maxDescriptionWidth, 0);

        description.SizeFlagsHorizontal = SizeFlags.ShrinkBegin;
        description.SizeFlagsVertical = SizeFlags.ShrinkBegin;
    }

    public void ChangeData(string titleText, string descriptionText)
    {
        title.Text = titleText;
        description.Text = descriptionText;
    }

    public override void _Process(double delta)
    {
        if(isVisible)
        {
            Vector2 mousePos = GetViewport().GetMousePosition();

            GlobalPosition = mousePos + new Vector2(16, 16);
        }
    }

    public void OnShow()
    {
        Visible = true;
        isVisible = true;
    }
    public void OnHide()
    {
        Visible = false;
        isVisible = false;
    }
}
