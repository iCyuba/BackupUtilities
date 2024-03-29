using BackupUtilities.Config.Components.Generic;
using BackupUtilities.Config.Components.Views;
using BackupUtilities.Config.Nodes;
using BackupUtilities.Config.Yoga;
using BackupUtilities.Shared;

namespace BackupUtilities.Config.Components.Job;

/// <summary>
/// List of cards representing backup jobs.
/// </summary>
public class CardList : LeftRightView
{
    public event Action? Updated;

    private readonly NewButton _newButton = new();
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
                card.Deleted -= Remove;
            }

            _cards = value.Select((j, i) => new Card(j) { Index = i }).ToList();

            if (_cards.Count != 0)
                _container.SetChildren(_cards.Select(c => c.Node));
            _container.InsertChild(_newButton.Node, _container.ChildCount); // The previous line will remove the button

            foreach (var card in _cards)
            {
                card.Register(this);
                card.Updated += Update;
                card.Deleted += Remove;
            }

            // Try focusing the first card. This will focus the button if no cards are available.
            FocusNext();
        }
    }

    public bool Valid => _cards.All(c => c.Valid) && _cards.Count != 0;

    public CardList(IEnumerable<BackupJob> jobs)
    {
        _newButton.Register(this);

        _cards = [];
        Jobs = jobs;

        _container.FlexDirection = FlexDirection.Row;
        _container.AlignItems = Align.FlexEnd;
        _container.FlexWrap = Wrap.Wrap;
        _container.Overflow = Overflow.Scroll;
        _container.SetGap(Gutter.Column, 2);
        _container.SetGap(Gutter.Row, 1);

        _newButton.Button.Clicked += Add;
    }

    private void Update() => Updated?.Invoke();

    private void Add()
    {
        var job = new BackupJob();
        Card card = new(job) { Index = _cards.Count };

        card.Register(this);
        card.Updated += Update;
        card.Deleted += Remove;
        _container.InsertChild(card.Node, _cards.Count);
        _cards.Add(card);

        // Focus the new card
        Focus(card);

        Update();
    }

    private void Remove()
    {
        var card = (Card)Active!;

        FocusNearest();

        _container.RemoveChild(card.Node);
        _cards.Remove(card);
        card.Unregister();
        card.Updated -= Update;
        card.Deleted -= Remove;

        // Update the indices
        for (var i = 0; i < _cards.Count; i++)
            _cards[i].Index = i;

        Update();
    }

    protected override void OnFocusChange(IInteractive? old, IInteractive? current)
    {
        base.OnFocusChange(old, current);

        // Scroll to the selected card
        if (current != null)
            if (current is Card card)
                _container.ScrollTo = card.Node;
            else if (current is Button)
                _container.ScrollTo = _newButton.Node;
    }
}
