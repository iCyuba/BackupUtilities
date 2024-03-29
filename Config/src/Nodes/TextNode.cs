using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Nodes;

/// <summary>
/// A node that renders text in the console.
/// </summary>
public class TextNode : RenderableNode
{
    private string _text;

    /// <summary>
    /// The text to display in the console.
    ///
    /// <br/>
    ///
    /// Note: ANSI escape codes are not supported, and will break due to line breaking.
    /// </summary>
    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            MarkDirty();
        }
    }

    public Color? Color { get; set; }

    public Color? BackgroundColor { get; set; }

    public bool Bold { get; set; }

    public bool Italic { get; set; }

    public bool Underline { get; set; }

    public bool Strikethrough { get; set; }

    public bool Trim { get; set; } = true;

    public TextNode(string text)
        : base()
    {
        _text = text;

        // Set the measure function
        base.MeasureFunc = MeasureFunc;
    }

    public override string ToString() => Text;

    /// <summary>
    /// Characters that can be used to split words
    /// </summary>
    private static readonly char[] _splitChars =
    [
        '-',
        '_',
        '/',
        '\\',
        ':',
        '\x200B', /* 0-width space */
        '\n',
    ];

    /// <summary>
    /// Checks if a character can be used to split words
    /// </summary>
    private static bool CanSplit(char c) => char.IsWhiteSpace(c) || _splitChars.Contains(c);

    /// <summary>
    /// Wraps the text to fit within the specified width.
    ///
    /// Uses the wcwidth library to measure the width of the text,
    /// </summary>
    /// <param name="width">The width to wrap the text to, -1 for no wrapping</param>
    /// <returns>The wrapped text and the maximum line width</returns>
    private (string[] lines, int width) WrapText(int width = -1)
    {
        if (width == 0)
            return ([], 0);

        if (width == -1)
            width = int.MaxValue;

        List<string> lines = [];

        int maxLineWidth = 0;

        string line = "";
        int lineWidth = 0;

        // Count the number of lines
        for (int i = 0; i < Text.Length; i++)
        {
            char character = Text[i];

            // Character = split char -> add it and continue
            if (CanSplit(character))
            {
                AddChar(character);
                continue;
            }

            // Get the index of the last character in the current word, the word and its width
            int wordEnd = i;
            for (; wordEnd < Text.Length; wordEnd++)
                if (CanSplit(Text[wordEnd]))
                    break;

            string word = Text[i..wordEnd];
            int wordWidth = word.Width();

            // If the word is longer than the remaining space and the line isn't empty, reset line
            // If max height is reached, only add 3 dots and return
            if (lineWidth != 0 && lineWidth + wordWidth > width)
                ResetLine();

            // Add all characters in the word to the current line
            foreach (char c in word)
                AddChar(c);

            // Set the i to the end of the word - 1
            i = wordEnd - 1;
        }

        // Add the last line
        ResetLine();

        return (lines.ToArray(), maxLineWidth);

        void ResetLine()
        {
            // Trim the end if trimming is enabled
            lines.Add(Trim ? line.Trim() : line);

            // Update the maximum line width
            maxLineWidth = int.Max(maxLineWidth, lineWidth);

            line = "";
            lineWidth = 0;
        }

        void AddChar(char c)
        {
            // If the character is a newline, reset the line (twice)
            if (c == '\n')
            {
                ResetLine();
                lines.Add("");

                return;
            }

            // Control characters shouldn't have negative width
            int cWidth = int.Max(0, c.Width());

            if (lineWidth + cWidth > width)
                ResetLine();

            // If the line is empty and the character is whitespace, ignore it
            if (lineWidth == 0 && char.IsWhiteSpace(c) && Trim)
                return;

            // Render all whitespace as a space
            if (char.IsWhiteSpace(c))
                c = ' ';

            lineWidth += cWidth;
            line += c;
        }
    }

    /// <summary>
    /// Implementation of the Yoga measure function for text nodes in the console.
    /// </summary>
    private static new Size MeasureFunc(
        Node node,
        float width,
        MeasureMode widthMode,
        float height,
        MeasureMode heightMode
    )
    {
        if (node is not TextNode textNode)
            throw new InvalidOperationException("Expected a TextNode");

        (string[] lines, int maxLineWidth) = textNode.WrapText(
            widthMode == MeasureMode.Undefined ? -1 : (int)width
        );

        return new(
            maxLineWidth,
            heightMode switch
            {
                MeasureMode.Exactly => height,
                _ => lines.Length
            }
        );
    }

    protected override RenderOutput Render()
    {
        int width = (int)ComputedWidth;
        int height = (int)ComputedHeight;

        (string[] lines, _) = WrapText(width);

        var buffer = new Character[height, width];

        for (int y = 0; y < height; y++)
        {
            if (y < lines.Length)
            {
                string line = lines[y];
                int x = 0;

                foreach (char c in line)
                {
                    buffer[y, x].Value ??= "";
                    buffer[y, x].Value += c;

                    // Styles
                    buffer[y, x].Foreground = Color;
                    buffer[y, x].Bold = Bold;
                    buffer[y, x].Italic = Italic;
                    buffer[y, x].Underline = Underline;
                    buffer[y, x].Strikethrough = Strikethrough;

                    x += int.Max(0, c.Width());
                }
            }

            // Set the bg color after the text has been rendered
            for (int x = 0; x < width; x++)
                buffer[y, x].Background = BackgroundColor;
        }

        if (lines.Length > height && height >= 1)
            for (int x = 1; x <= int.Min(3, width); x++)
                buffer[height - 1, width - x].Value = ".";

        // Value nodes can't have children, so the base method isn't called here.
        return new(new(buffer, default), RenderBuffer.Empty);
    }
}
