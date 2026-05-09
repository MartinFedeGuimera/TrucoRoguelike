using Godot;

public partial class DescriptionController : Control
{
    private Label title;
    private Label description;

    private bool isVisible = false;

    public void SetUp(string titleText, string descriptionText)
    {
        title = GetNode<Label>("MarginContainer/VBoxContainer/Title");
        description = GetNode<Label>("MarginContainer/VBoxContainer/Description");

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
