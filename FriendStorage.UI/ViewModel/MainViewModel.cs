using FriendStorage.UI.Events;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;

namespace FriendStorage.UI.ViewModel
{
public class MainViewModel : ViewModelBase
{
    private readonly Func<IFriendEditViewModel> _friendEditViewModelCreator;
    private readonly IEventAggregator _eventAggregator;
    private IFriendEditViewModel _selectedFriendEditViewModel;

    public MainViewModel(INavigationViewModel navigationViewModel,
                         Func<IFriendEditViewModel> friendEditViewModelCreator,
                         IEventAggregator eventAggregator)
    {
        NavigationViewModel = navigationViewModel;
        _friendEditViewModelCreator = friendEditViewModelCreator;
        _eventAggregator = eventAggregator;

        FriendEditViewModels = new ObservableCollection<IFriendEditViewModel>();
        CloseFriendTabCommand = new DelegateCommand<IFriendEditViewModel>(OnCloseFriendTabExecute);

        _eventAggregator.GetEvent<OpenFriendEditViewEvent>()
            .Subscribe(OnOpenFriendEditViewEvent);
    }

    public ObservableCollection<IFriendEditViewModel> FriendEditViewModels { get; private set; }
    public INavigationViewModel NavigationViewModel { get; private set; }
    public IFriendEditViewModel SelectedFriendEditViewModel
    {
        get => _selectedFriendEditViewModel;
        set
        {
            _selectedFriendEditViewModel = value; 
            OnPropertyChanged();
        }
    }

    public ICommand CloseFriendTabCommand { get; }

    public void Load() =>
        NavigationViewModel.Load();

    private void OnOpenFriendEditViewEvent(int friendId)
    {
        var friendEditViewModel = FriendEditViewModels.SingleOrDefault(s => s.Friend.Id == friendId);
        if (friendEditViewModel == null)
        {
            friendEditViewModel = _friendEditViewModelCreator();
            FriendEditViewModels.Add(friendEditViewModel);
            friendEditViewModel.Load(friendId);
        }
        SelectedFriendEditViewModel = friendEditViewModel;
    }
    private void OnCloseFriendTabExecute(IFriendEditViewModel friendEditViewModel)
    {
        FriendEditViewModels.Remove(friendEditViewModel);
        //SelectedFriendEditViewModel = null;
    }


}
}
