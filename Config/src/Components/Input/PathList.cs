using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Util;
using BackupUtilities.Config.Yoga;
using NativeFileDialogSharp;

namespace BackupUtilities.Config.Components.Input;

/// <summary>
/// A list of path inputs.
/// </summary>
public class PathList : List<string, TextBox>
{
    private sealed class PathListInput : ListInput
    {
        protected override int Padding => 12;

        private readonly Button _browse = new("");

        public PathListInput()
        {
            Node.InsertChild(_browse.Node, 1);
            _browse.Register(this);

            _browse.Clicked += Browse;

            UpdateStyle();
        }

        public void Browse()
        {
            try
            {
                var folder = Dialog.FolderPicker();

                if (!folder.IsOk)
                    return;

                Value = folder.Path;
                Update();
            }
            catch { }
        }

        protected override void UpdateStyle()
        {
            base.UpdateStyle();
            _browse.Node.Display = IsFocused ? Display.Flex : Display.None;
        }
    }

    /// <summary>
    /// Factory for creating path list inputs.
    /// </summary>
    private class PathListInputFactory : IFactory<ListInput>
    {
        public ListInput Create() => new PathListInput();
    }

    public PathList() => Factory = new PathListInputFactory();

    protected override void Add()
    {
        base.Add();

        if (InputContainer.Active is PathListInput input)
            input.Browse();
    }
}
