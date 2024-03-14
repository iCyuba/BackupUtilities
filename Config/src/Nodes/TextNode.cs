﻿using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;

namespace BackupUtilities.Config.Nodes;

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

    public bool Bold { get; set; }

    public bool Italic { get; set; }

    public bool Underline { get; set; }

    public bool Strikethrough { get; set; }

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
    /// <param name="width">The width to wrap the text to</param>
    /// <returns>The wrapped text and the maximum line width</returns>
    private (string[] lines, int width) WrapText(int width)
    {
        if (width == 0)
            return ([], 0);

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
            // Trim the end
            lines.Add(line.TrimEnd());

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

            // Render all whitespace as
            // If the line is empty and the character is whitespace, ignore it
            if (lineWidth == 0 && char.IsWhiteSpace(c))
                return;

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

        int strW = textNode.Text.Width();

        // Width mode = undefined -> return the string width
        // Width mode = at most && text < width -> also return the string width
        // Width mode = exactly && text is smaller than width -> return the input width
        // Width mode & height mode = exactly -> return the input width and height
        if (
            widthMode == MeasureMode.Undefined
            || strW < width
            || (widthMode == heightMode && heightMode == MeasureMode.Exactly)
        )
            return new(
                widthMode switch
                {
                    MeasureMode.Exactly => width,
                    _ => strW
                },
                heightMode switch
                {
                    MeasureMode.Exactly => height,
                    _ => 1
                }
            );

        // Width mode = at most | exactly && text is longer than width -> calculate the number of lines required
        (string[] lines, int maxLineWidth) = textNode.WrapText((int)width);

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

        var buffer = new string[height, width];

        for (int y = 0; y < height; y++)
            if (y < lines.Length)
            {
                string line = lines[y];

                int x = 0;

                foreach (char c in line)
                {
                    if (Color != null)
                        buffer[y, x] += Color?.ToANSI(Util.Color.Target.Foreground);
                    if (Bold)
                        buffer[y, x] += "\x1b[1m";

                    if (Italic)
                        buffer[y, x] += "\x1b[3m";

                    if (Underline)
                        buffer[y, x] += "\x1b[4m";

                    if (Strikethrough)
                        buffer[y, x] += "\x1b[9m";

                    buffer[y, x] += c.ToString();
                    buffer[y, x] += "\x1b[0m";

                    x += int.Max(0, c.Width());
                }
            }

        if (lines.Length > height)
            for (int x = 1; x <= int.Min(3, width); x++)
                buffer[height - 1, width - x] = ".";

        // Text nodes can't have children, so the base method isn't called here.
        return new(buffer, (0, 0), []);
    }
}