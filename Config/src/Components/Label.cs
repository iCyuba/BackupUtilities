using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Components;

public class Label : BaseComponent
{
    public class Content : BaseComponent
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

        public Color? Color
        {
            get => _text.Color;
            set => _text.Color = value;
        }

        public Color? BackgroundColor
        {
            get => Node.Color;
            set => Node.Color = value;
        }

        public Content(string text)
        {
            _text = new(text) { Color = Color.White };
            Node.InsertChild(_text, 0);

            BackgroundColor = Color.Pink.Regular;

            Node.SetBorder(Edge.Horizontal, 1);
            Node.SetPadding(Edge.Horizontal, 1);
        }
    }

    public Label() => Node.Height = new(1);

    public override void AddChild(IComponent child, bool insert = true, bool update = true)
    {
        var children = Children.ToArray();

        if (child is not Content || children.Length == 0)
        {
            base.AddChild(child, insert, update);
            return;
        }

        children[^1].Node.SetPadding(Edge.Right, 2);
        child.Node.SetMargin(Edge.Left, -2);

        base.AddChild(child, insert, update);
    }
}
