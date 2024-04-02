using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Input;

/// <summary>
/// A radio input for selecting a single value from a set of options.
/// </summary>
/// <typeparam name="T">Enum type of the radio values.</typeparam>
public sealed class Radio<T> : BaseView, IInput<T>
    where T : struct, Enum
{
    /// <summary>
    /// A single radio value. Used by the parent component.
    /// </summary>
    private sealed class RadioValue : BaseInteractive
    {
        protected override IEnumerable<IComponent> SubComponents => [_label];

        private readonly Label _label;
        private readonly Label.TextContent _icon = new(" ");
        private readonly Label.TextContent _text;

        public override RenderableNode Node => _label.Node;

        public T Value { get; }

        public RadioValue(T value)
        {
            Value = value;
            _text = new(value.GetDescription())
            {
                Style = Label.Content.ContentStyle.None,
                Bold = true,
            };

            _label = new([_icon, _text], true);

            Focused += UpdateStyle;
            Blurred += UpdateStyle;

            UpdateStyle();
        }

        protected override void UpdateStyle()
        {
            _text.Bold = IsFocused;
            _text.Color = IsFocused ? Color.Foreground.Primary : Color.Foreground.Secondary;
            _icon.BackgroundColor = IsFocused ? Color.Primary.Primary : Color.Element.Secondary;
            _icon.Text = IsFocused ? "" : " ";
        }
    }

    public event Action? Updated;

    private T _value;
    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            Focus(_radios.Single((r) => r.Value.Equals(value)));
        }
    }

    private readonly RadioValue[] _radios;

    private readonly FancyNode _container = new() { FlexDirection = FlexDirection.Column };
    public override RenderableNode Node => _container;

    public Radio()
    {
        var values = Enum.GetValues<T>();

        _radios = values.Select(value => new RadioValue(value)).ToArray();
        foreach (var radio in _radios)
            radio.Register(this);

        _container.SetGap(Gutter.Row, 1);
        _container.SetChildren(_radios.Select(r => r.Node));
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.UpArrow)
            FocusPrevious();
        else if (key.Key == ConsoleKey.DownArrow)
            FocusNext();
        else
            base.HandleInput(key);
    }

    protected override void OnFocusChange(IInteractive? old, IInteractive? current)
    {
        base.OnFocusChange(old, current);

        if (current is RadioValue radio)
        {
            _value = radio.Value;
            Updated?.Invoke();
        }
    }
}
