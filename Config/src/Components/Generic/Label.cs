using BackupUtilities.Config.Components.Base;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components.Generic;

/// <summary>
/// Customizable label. Used by a lot of other components.
/// </summary>
public sealed class Label : BaseComponent
{
    /// <summary>
    /// Empty label content. Used for spacing.
    /// </summary>
    public class Content : BaseComponent
    {
        public enum ContentStyle
        {
            None,
            Regular,
        }

        public event Action? Updated;

        protected FancyNode Container { get; } = new();

        public override RenderableNode Node => Container;

        private readonly ContentStyle _style = ContentStyle.Regular;
        public ContentStyle Style
        {
            get => _style;
            init
            {
                _style = value;
                UpdateStyle();
            }
        }

        private Color? _backgroundColor = Color.Primary.Primary;
        public Color? BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                _backgroundColor = value;
                UpdateStyle();
            }
        }

        public Content()
        {
            Container.FlexGrow = 1;

            Container.SetBorder(Edge.Horizontal, 1);
            Container.SetPadding(Edge.Horizontal, 1);
            Container.BackgroundColor = BackgroundColor;
        }

        protected override void UpdateStyle()
        {
            Container.SetBorder(Edge.Horizontal, (float)Style);
            Container.BackgroundColor = Style == ContentStyle.Regular ? BackgroundColor : null;

            Updated?.Invoke();
        }
    }

    /// <summary>
    /// Text content. Used for displaying text and icons.
    /// </summary>
    public class TextContent : Content
    {
        private readonly TextNode _text;

        public string Text
        {
            get => _text.Text;
            set => _text.Text = value;
        }

        public bool Bold
        {
            get => _text.Bold;
            set => _text.Bold = value;
        }

        public bool Strikethrough
        {
            get => _text.Strikethrough;
            set => _text.Strikethrough = value;
        }

        private Color? _color = Color.White;
        public Color? Color
        {
            get => _color;
            set
            {
                _color = value;
                UpdateStyle();
            }
        }

        public TextContent(string text)
        {
            Container.FlexGrow = 0;

            _text = new(text) { Trim = false };
            Container.InsertChild(_text, 0);

            _text.Color = Color;
        }

        protected override void UpdateStyle()
        {
            base.UpdateStyle();

            _text.Color = _color;
        }
    }

    protected override IEnumerable<IComponent> SubComponents => Children;

    private readonly FancyNode _container = new();
    public override RenderableNode Node => _container;

    private Content[] _children = [];
    public IEnumerable<Content> Children
    {
        get => _children.AsReadOnly();
        set
        {
            _children = value.ToArray();
            _container.SetChildren(_children.Select(c => c.Node));

            UpdateStyle();
        }
    }

    private bool _gap;
    public bool Gap
    {
        get => _gap;
        set
        {
            _gap = value;

            UpdateStyle();
        }
    }

    public Label(IEnumerable<Content> children, bool gap = false)
    {
        _children = children.ToArray();
        _container.SetChildren(_children.Select(c => c.Node));

        Gap = gap;
    }

    protected override void UpdateStyle()
    {
        for (var i = 0; i < _children.Length - 1; i++)
        {
            var one = _children[i];
            var two = _children[i + 1];

            bool gap =
                Gap
                || one.Style == Content.ContentStyle.None
                || two.Style == Content.ContentStyle.None;

            one.Node.SetPadding(Edge.Right, gap ? 1 : 2);
            two.Node.SetMargin(Edge.Left, gap ? 0 : -2);
        }
    }
}
