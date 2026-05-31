using Godot;
using Godot.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public partial class DescriptionController : Control
{
    private Label title;
    private RichTextLabel description;
    private MarginContainer margin;

    private bool isVisible = false;

    [Export] private float maxDescriptionWidth = 300f;

    public void SetUp()
    {
        SetAnchorsPreset(LayoutPreset.TopLeft);

        title = GetNode<Label>("MarginContainer/VBoxContainer/Title");
        description = GetNode<RichTextLabel>("MarginContainer/VBoxContainer/Description");
        margin = GetNode<MarginContainer>("MarginContainer");

        description.AutowrapMode = TextServer.AutowrapMode.Word;
        description.CustomMinimumSize = new Vector2(maxDescriptionWidth, 0);
    }

    public async void ChangeData(string titleText, string descriptionText, System.Collections.Generic.Dictionary<string, object> vars)
    {
        title.Text = titleText;

        string finalDescription = DescriptionFormatter.Format(descriptionText, vars);

        description.Text = finalDescription;

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        ResetSize();
    }

    public override void _Process(double delta)
    {
        if (!isVisible)
            return;

        Vector2 mousePos = GetViewport().GetMousePosition();
        Vector2 viewportSize = GetViewportRect().Size;

        Vector2 pos = mousePos + new Vector2(16, 16);

        if (pos.X + margin.Size.X > viewportSize.X)
            pos.X = mousePos.X - margin.Size.X - 16;

        if (pos.Y + margin.Size.Y > viewportSize.Y)
            pos.Y = mousePos.Y - margin.Size.Y - 16;

        GlobalPosition = pos;
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
