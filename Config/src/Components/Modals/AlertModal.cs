using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Modals;

/// <summary>
/// A modal for showing an alert.
/// </summary>
public class AlertModal : BaseModal
{
    public string Text
    {
        get => _text.Text;
        set => _text.Text = value;
    }

    private readonly TextNode _text;
    private readonly Button _okay = new("", "Okay") { Color = Color.Slate };

    public AlertModal(string text)
    {
        Icon = "";
        Title = "Alert";

        _text = new TextNode(text) { Color = Color.Slate.Dark };

        Content.SetChildren([_text, _okay.Node]);
        Content.SetGap(Gutter.Row, 1);
        Content.FlexDirection = FlexDirection.Column;
        Content.AlignItems = Align.Center;

        _okay.Clicked += Close;
        _okay.Register(this);
    }
}
