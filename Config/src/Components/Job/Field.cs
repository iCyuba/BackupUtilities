using System.Linq.Expressions;
using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Input;
using BackupUtilities.Config.Components.Modals;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Job;

/// <summary>
/// A field for editing a property of a job.
/// </summary>
/// <typeparam name="T">The type of the property.</typeparam>
public sealed class Field<T> : BaseButton
{
    public event Action? Updated;

    protected override IEnumerable<IComponent> SubComponents => [_label];

    private readonly InputModal<T> _modal;

    private readonly Label _label;
    private readonly Label.TextContent _icon;
    private readonly Label.TextContent _text;
    private readonly Label.Content _separator = new() { Style = Label.Content.ContentStyle.None };
    private readonly Label.TextContent _validation =
        new("") { Style = Label.Content.ContentStyle.None, Color = Color.Yellow.Primary };
    private readonly Label.TextContent _preview = new("") { Bold = true };
    public override RenderableNode Node => _label.Node;

    private readonly PropertyRef<T> _value;
    private readonly Func<T, string> _previewFunc;
    private readonly Predicate<T>? _validateFunc;

    private string Preview => _previewFunc(_value.Value);

    public bool Valid => _validateFunc?.Invoke(_value.Value) ?? true;

    public Field(
        Expression<Func<T>> value,
        Func<T, string> preview,
        IInput<T> input,
        Predicate<T>? validate = null,
        bool closeOnEnter = true
    )
    {
        _value = new(value);
        _previewFunc = preview;
        _validateFunc = validate;

        _icon = new(Icon.GetIcon(_value.Property));
        _text = new(_value.Name)
        {
            Style = Label.Content.ContentStyle.None,
            Bold = true,
            Color = Color.Foreground.Primary
        };

        _label = new([_icon, _text, _separator, _validation, _preview]);
        _modal = new(input, closeOnEnter) { Title = _value.Name, Icon = _icon.Text };

        Clicked += OpenModal;
        _modal.Updated += UpdateValue;

        UpdateStyle();
    }

    protected override void UpdateStyle()
    {
        _icon.BackgroundColor = IsFocused ? Color.Primary.Primary : Color.Element.Secondary;
        _validation.Node.Display = Valid ? Yoga.Display.None : Yoga.Display.Flex;
        _preview.BackgroundColor = IsFocused ? Color.Element.Primary : Color.Element.Secondary;
        _preview.Text = Preview;
    }

    private void OpenModal()
    {
        _modal.Value = _value.Value;

        OpenModal(_modal);
    }

    private void UpdateValue()
    {
        _value.Value = _modal.Value;
        UpdateStyle();

        Updated?.Invoke();
    }
}
