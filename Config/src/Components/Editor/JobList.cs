using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Editor;

public class JobList : BaseView
{
    private readonly FancyNode _container = new();
    public override RenderableNode Node => _container;

    private List<(BackupJob job, JobCard component)> _jobs;
    public IEnumerable<BackupJob> Jobs
    {
        get => _jobs.Select(j => j.job).ToList();
        set
        {
            _jobs = value.Select((j, i) => (j, new JobCard(j) { Index = i })).ToList();

            _container.SetChildren(_jobs.Select(pair => pair.component.Node));

            foreach (var (_, component) in _jobs)
                component.Register(this);
        }
    }

    public JobList(IEnumerable<BackupJob> jobs)
    {
        _jobs = [];
        Jobs = jobs;

        _container.FlexDirection = FlexDirection.Row;
        _container.AlignItems = Align.FlexStart;
        _container.Overflow = Overflow.Scroll;
        _container.SetMargin(Edge.Left, 1);
        _container.SetGap(Gutter.Column, 2);
    }

    public override void HandleInput(ConsoleKeyInfo key)
    {
        if (key.Key == ConsoleKey.LeftArrow && !CapturesInput)
            FocusPrevious();
        else if (key.Key == ConsoleKey.RightArrow && !CapturesInput)
            FocusNext();
        else
            base.HandleInput(key);
    }
}
