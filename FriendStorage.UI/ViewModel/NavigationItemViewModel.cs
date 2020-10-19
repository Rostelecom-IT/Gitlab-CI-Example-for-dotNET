using System.Windows.Input;
using FriendStorage.UI.Events;
using Prism.Commands;
using Prism.Events;

namespace FriendStorage.UI.ViewModel
{
public class NavigationItemViewModel
{
    private readonly IEventAggregator _eventAggregator;

    public NavigationItemViewModel(int id, string displayMember, IEventAggregator eventAggregator)
    {
        _eventAggregator = eventAggregator;
        Id = id;
        DisplayMember = displayMember;
        OpenFriendEditViewCommand = new DelegateCommand(OnOpenFriendEditViewExecute);
    }

    public int Id { get; }
    public string DisplayMember { get; }
    public ICommand OpenFriendEditViewCommand { get; }

private void OnOpenFriendEditViewExecute() =>
    _eventAggregator.GetEvent<OpenFriendEditViewEvent>()
        .Publish(Id);
}
}
