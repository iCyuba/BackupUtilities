using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Modals;

/// <summary>
/// Modal for confirming an action.
/// </summary>
public class ConfirmModal : BaseModal
{
    public event Action? Confirmed;

    public string Text
    {
        get => _text.Text;
        set => _text.Text = value;
    }

    public bool Destructive
    {
        get => _yes.Accent.Equals(Color.Red);
        set => _yes.Accent = value ? Color.Red : Color.Primary;
    }

    private readonly TextNode _text;
    private readonly FancyNode _buttons = new() { JustifyContent = Justify.SpaceBetween };
    private readonly Button _no = new("", "No");
    private readonly Button _yes = new("", "Yes");

    public ConfirmModal(string text)
    {
        Icon = "";
        Title = "Confirm";

        _text = new TextNode(text) { Color = Color.Slate.Dark };

        _buttons.SetChildren([_no.Node, _yes.Node]);
        _buttons.SetGap(Gutter.Column, 1);

        Content.SetChildren([_text, _buttons]);
        Content.SetGap(Gutter.Row, 1);
        Content.FlexDirection = FlexDirection.Column;
        Content.AlignItems = Align.Center;

        _no.Clicked += Close;
        _no.Register(this);

        _yes.Clicked += Confirm;
        _yes.Register(this);
    }

    private void Confirm()
    {
        Confirmed?.Invoke();
        Close();
    }
}
