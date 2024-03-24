using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Job;

public class JobList : LeftRightView
{
    public event Action? Updated;

    private readonly FancyNode _container = new();
    public override RenderableNode Node => _container;

    private List<Card> _cards;
    public IEnumerable<BackupJob> Jobs
    {
        get => _cards.Select(c => c.Job).ToList();
        set
        {
            foreach (var card in _cards)
            {
                card.Unregister();
                card.Updated -= Update;
            }

            _cards = value.Select((j, i) => new Card(j) { Index = i }).ToList();

            _container.SetChildren(_cards.Select(c => c.Node));

            foreach (var card in _cards)
            {
                card.Register(this);
                card.Updated += Update;
            }

            // Try focusing the first card. This will focus the button if no cards are available.
            FocusNext();
        }
    }

    public bool Valid => _cards.All(c => c.Valid);

    public JobList(IEnumerable<BackupJob> jobs)
    {
        _cards = [];
        Jobs = jobs;

        _container.FlexDirection = FlexDirection.Row;
        _container.AlignItems = Align.FlexStart;
        _container.Overflow = Overflow.Scroll;
        _container.SetMargin(Edge.Left, 1);
        _container.SetGap(Gutter.Column, 2);
    }

    private void Update() => Updated?.Invoke();
}
