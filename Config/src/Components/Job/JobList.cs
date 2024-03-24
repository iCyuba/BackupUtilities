using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Job;

public class JobList : LeftRightView
{
    public event Action? Updated;

    private readonly NewButton _button = new();
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
            _container.InsertChild(_button.Node, _container.ChildCount); // The previous line will remove the button

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
        _button.Register(this);

        _cards = [];
        Jobs = jobs;

        _container.FlexDirection = FlexDirection.Row;
        _container.AlignItems = Align.FlexEnd;
        _container.FlexWrap = Wrap.Wrap;
        _container.Overflow = Overflow.Scroll;
        _container.SetPadding(Edge.Left, 1);
        _container.SetGap(Gutter.Column, 2);
        _container.SetGap(Gutter.Row, 1);

        _button.Clicked += Add;
    }

    private void Update() => Updated?.Invoke();

    private void Add()
    {
        var job = new BackupJob();
        Card card = new(job) { Index = _cards.Count };

        card.Register(this);
        card.Updated += Update;
        _container.InsertChild(card.Node, _cards.Count);
        _cards.Add(card);

        // Focus the new card
        Focus(card);

        Update();
    }

    protected override void OnFocusChange(IInteractive? old, IInteractive? current)
    {
        base.OnFocusChange(old, current);

        // Scroll to the selected card
        if (current != null)
            _container.ScrollTo = current.Node;
    }
}
