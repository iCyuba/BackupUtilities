using System.Linq.Expressions;
using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Modals;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Job;

public sealed class Field<T> : BaseButton
{
    protected override IEnumerable<IComponent> SubComponents =>
        [_label, _icon, _text, _separator, _preview];

    private readonly InputModal<T> _modal;

    private readonly Label _label;
    private readonly Label.TextContent _icon;
    private readonly Label.TextContent _text;
    private readonly Label.Content _separator = new() { Style = Label.Content.ContentStyle.None };
    private readonly Label.TextContent _preview = new("") { Bold = true };
    public override RenderableNode Node => _label.Node;

    private readonly PropertyRef<T> _value;
    private readonly Func<T, string> _previewFunc;

    public Field(Expression<Func<T>> value, Func<T, string> preview, IInput<T> input)
    {
        _value = new(value);
        _previewFunc = preview;

        _icon = new(Icon.GetIcon(_value.Property));
        _text = new(_value.Name)
        {
            Style = Label.Content.ContentStyle.None,
            Bold = true,
            Color = Color.Slate.Dark
        };

        _label = new() { Children = [_icon, _text, _separator, _preview] };

        _modal = new(input);

        Clicked += OpenModal;
        _modal.Updated += UpdateValue;
        _modal.Closed += CloseModal;

        _modal.Title.Text = _value.Name;
        _modal.Title.Icon = _icon.Text;

        UpdateStyle();
    }

    protected override void UpdateStyle()
    {
        _icon.BackgroundColor = IsFocused ? Color.Primary.Dark : Color.Slate.Light;
        _preview.BackgroundColor = IsFocused ? Color.Slate.Regular : Color.Slate.Light;
        _preview.Text = _previewFunc(_value.Value);
    }

    private void OpenModal()
    {
        _modal.Value = _value.Value;

        Node.InsertChild(_modal.Node, Node.ChildCount);
        _modal.Register(View!);

        View!.Focus(_modal);
    }

    private void UpdateValue()
    {
        _value.Value = _modal.Value;
        UpdateStyle();
    }

    private void CloseModal()
    {
        Node.RemoveChild(_modal.Node);
        _modal.Unregister();

        View!.Focus(this);
    }
}
