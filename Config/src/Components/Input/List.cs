using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Input;

/// <summary>
/// A list of inputs.
/// </summary>
/// <typeparam name="TValue">Type of the inputs' values.</typeparam>
/// <typeparam name="TInput">Type of the input components.</typeparam>
public class List<TValue, TInput> : BaseComponent, IInput<IEnumerable<TValue>>
    where TInput : class, IInput<TValue>, new()
{
    /// <summary>
    /// Helper class for containing the list of inputs. Required for focus navigation.
    /// </summary>
    protected class ListContainer : UpDownView
    {
        private readonly FancyNode _container =
            new()
            {
                FlexDirection = FlexDirection.Column,
                Width = new(Unit.Percent, 100),
                Overflow = Overflow.Scroll
            };
        public override RenderableNode Node => _container;

        public ListContainer() => _container.SetGap(Gutter.Row, 1);

        public void AddChild(RenderableNode child) =>
            _container.InsertChild(child, _container.ChildCount);

        public void RemoveChild(RenderableNode child) => _container.RemoveChild(child);

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

            if (current == null)
                return;

            _container.ScrollTo = current.Node;
        }
    }

    /// <summary>
    /// Helper class for the input and the remove button.
    /// </summary>
    protected class ListInput : LeftRightView
    {
        public event Action? Updated;
        public event Action? Removed;

        public TValue Value
        {
            get => _input.Value;
            set => _input.Value = value;
        }

        protected virtual int Padding => 6;

        private readonly Button _remove = new("") { Accent = Color.Red };
        private readonly TInput _input = new();

        private readonly FancyNode _container =
            new()
            {
                AlignItems = Align.FlexEnd,
                FlexGrow = 1,
                FlexShrink = 0
            };

        public override RenderableNode Node => _container;

        public ListInput()
        {
            _container.SetGap(Gutter.Column, 1);
            _container.SetChildren([_input.Node, _remove.Node]);

            // Is the order wrong here intentionally? no. will it break without it? yes.
            _remove.Register(this);
            _input.Register(this);

            _input.Updated += Update;
            _remove.Clicked += Remove;

            _remove.Node.FlexShrink = 0;

            if (_input is IInteractive interactive)
                Focused += () => Focus(interactive);

            Focused += UpdateStyle;
            Blurred += UpdateStyle;

            _remove.Node.Display = Display.None;
            _input.Node.SetPadding(Edge.Right, 6);
        }

        protected override void UpdateStyle()
        {
            _remove.Node.Display = IsFocused ? Display.Flex : Display.None;
            _input.Node.SetPadding(Edge.Right, IsFocused ? 0 : Padding);
        }

        protected void Update() => Updated?.Invoke();

        private void Remove() => Removed?.Invoke();
    }

    /// <summary>
    /// Default factory for the input components.
    /// </summary>
    private class ListInputFactory : IFactory<ListInput>
    {
        public ListInput Create() => new();
    }

    public event Action? Updated;

    protected IFactory<ListInput> Factory { get; init; } = new ListInputFactory();

    public IEnumerable<TValue> Value
    {
        get => _inputs.Select(input => input.Value);
        set
        {
            var inputs = value.ToArray();

            // Ensure the correct number of inputs.
            if (_inputs.Count > inputs.Length)
            {
                for (var i = inputs.Length; i < _inputs.Count; i++)
                {
                    _inputs[i].Updated -= Update;
                    _inputs[i].Removed -= Remove;
                    _inputs[i].Unregister();
                    InputContainer.RemoveChild(_inputs[i].Node);
                }

                _inputs.RemoveRange(inputs.Length, _inputs.Count - inputs.Length);
            }
            else if (_inputs.Count < inputs.Length)
            {
                var newInputs = Enumerable
                    .Range(_inputs.Count, inputs.Length - _inputs.Count)
                    .Select(_ => Factory.Create())
                    .ToArray();

                foreach (var input in newInputs)
                {
                    input.Register(InputContainer);
                    InputContainer.AddChild(input.Node);
                    input.Updated += Update;
                    input.Removed += Remove;
                }

                _inputs.AddRange(newInputs);
            }

            // Update the inputs' values.
            for (var i = 0; i < inputs.Length; i++)
                _inputs[i].Value = inputs[i];

            // Update focus.
            if (_inputs.Count > 0)
                View?.Focus(InputContainer);
            else
                View?.Focus(_add);
        }
    }

    protected override IEnumerable<IComponent> SubComponents => [_add, InputContainer];

    private readonly FancyNode _container =
        new()
        {
            FlexDirection = FlexDirection.Column,
            AlignItems = Align.Center,
            FlexGrow = 1
        };

    private readonly Button _add = new("", "Add");

    protected readonly ListContainer InputContainer = new();
    public override RenderableNode Node => _container;

    private readonly List<ListInput> _inputs = [];

    public List()
    {
        _container.SetGap(Gutter.Row, 1);
        _container.SetChildren([InputContainer.Node, _add.Node]);

        _add.Clicked += Add;
    }

    private void Update() => Updated?.Invoke();

    protected virtual void Add()
    {
        var input = Factory.Create();
        input.Register(InputContainer);
        input.Updated += Update;
        input.Removed += Remove;

        InputContainer.AddChild(input.Node);
        _inputs.Add(input);

        if (input is IInteractive interactive)
        {
            View?.Focus(InputContainer);
            InputContainer.Focus(interactive);
        }

        Updated?.Invoke();
    }

    private void Remove()
    {
        if (InputContainer.Active == null)
            return;

        var input = (ListInput)InputContainer.Active;

        InputContainer.FocusNearest();

        input.Updated -= Update;
        input.Removed -= Remove;
        input.Unregister();

        InputContainer.RemoveChild(input.Node);
        _inputs.Remove(input);

        Updated?.Invoke();

        if (_inputs.Count == 0)
            View?.Focus(_add);
    }
}
