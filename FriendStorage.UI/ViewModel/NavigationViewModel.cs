using FriendStorage.DataAccess;
using FriendStorage.Model;
using System.Collections.ObjectModel;
using FriendStorage.UI.DataProvider;
using Prism.Events;

namespace FriendStorage.UI.ViewModel
{
public interface INavigationViewModel
{
    void Load();
}
public class NavigationViewModel : ViewModelBase, INavigationViewModel
{
    private readonly INavigationDataProvider _dataProvider;
    private readonly IEventAggregator _eventAggregator;

    public NavigationViewModel(INavigationDataProvider dataProvider, IEventAggregator eventAggregator)
    {
        _dataProvider = dataProvider;
        _eventAggregator = eventAggregator;
        Friends = new ObservableCollection<NavigationItemViewModel>();
    }

    public void Load()
    {
        Friends.Clear();
        foreach (var friend in _dataProvider.GetAllFriends())
        {
            Friends.Add(new NavigationItemViewModel(friend.Id, friend.DisplayMember, _eventAggregator));
        }
    }

    public ObservableCollection<NavigationItemViewModel> Friends { get; private set; }
}
}
