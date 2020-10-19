using FriendStorage.Model;
using FriendStorage.UI.Events;
using FriendStorage.UI.ViewModel;
using FriendStorage.UITests.Extensions;
using Moq;
using Prism.Events;
using System.Collections.Generic;
using System.Linq;
using FriendStorage.UI.Wrapper;
using Xunit;

namespace FriendStorage.UITests.ViewModel
{
public class MainViewModelTests
{
    private readonly MainViewModel _mainViewModel;
    private readonly Mock<INavigationViewModel> _navigationViewModelMoq;
    private readonly OpenFriendEditViewEvent _openFriendEditViewEvent;
    private Mock<IEventAggregator> _eventAggregatorMoq;
    private readonly List<Mock<IFriendEditViewModel>> _friendEditViewModelMoqs;

    public MainViewModelTests()
    {
        _friendEditViewModelMoqs = new List<Mock<IFriendEditViewModel>>();
        _navigationViewModelMoq = new Mock<INavigationViewModel>();

        _openFriendEditViewEvent = new OpenFriendEditViewEvent(); // Real Event
        _eventAggregatorMoq = new Mock<IEventAggregator>();
        _eventAggregatorMoq
            .Setup(s => s.GetEvent<OpenFriendEditViewEvent>())
            .Returns(_openFriendEditViewEvent);

        _mainViewModel = new MainViewModel(_navigationViewModelMoq.Object,
            CreateFriendEditViewModel,
            _eventAggregatorMoq.Object);
    }

    private IFriendEditViewModel CreateFriendEditViewModel()
    {
        var friendEditViewModelMoq = new Mock<IFriendEditViewModel>();
        friendEditViewModelMoq.Setup(s => s.Load(It.IsAny<int>()))
            .Callback<int>(friendId =>
                {
                    friendEditViewModelMoq.Setup(s => s.Friend)
                        .Returns(new FriendWrapper(new Friend{ Id = friendId }));
                });
        _friendEditViewModelMoqs.Add(friendEditViewModelMoq);
        return friendEditViewModelMoq.Object;
    }

    [Fact]
    public void ShouldCallLoadMethodOfNavigationViewModel()
    {
        //Assert.False(navigationViewModel.HasLoaded);
        _mainViewModel.Load();
        //Assert.True(navigationViewModel.HasLoaded);
        _navigationViewModelMoq.Verify(vm => vm.Load(), Times.Once);
    }

    [Fact]
    public void ShouldAddFriendEditViewModelAndLoadAndSelectIt()
    {
        const int friendId = 7;
        _openFriendEditViewEvent.Publish(friendId); // Publish Real Event
                                                    // Now in _
        Assert.Single(_mainViewModel.FriendEditViewModels);
        var firstEditVm = _mainViewModel.FriendEditViewModels.First();
        Assert.Equal(_mainViewModel.SelectedFriendEditViewModel, firstEditVm);
        _friendEditViewModelMoqs.First()
            .Verify(vm => vm.Load(friendId), Times.Once);
    }

    [Fact]
    public void ShouldAddFriendEditViewModelOnlyOnce()
    {
        _openFriendEditViewEvent.Publish(5);
        _openFriendEditViewEvent.Publish(5);
        _openFriendEditViewEvent.Publish(3);
        _openFriendEditViewEvent.Publish(7);
        _openFriendEditViewEvent.Publish(7);

        Assert.Equal(3, _mainViewModel.FriendEditViewModels.Count);
    }

[Fact]
public void ShouldRisePropertyChangedEventForSelectedFriendEditViewModel()
{
    var fired = _mainViewModel
        .IsPropertyChangedFired(() =>
            {
                var friendEditViewModel = new Mock<FriendEditViewModel>(new object[] {null});
                _mainViewModel.SelectedFriendEditViewModel = friendEditViewModel.Object;
            },
        nameof(_mainViewModel.SelectedFriendEditViewModel));

    Assert.True(fired);
}

    [Fact]
    public void ShouldRemoveFriendEditViewModelAfterCloseTab()
    {
        _openFriendEditViewEvent.Publish(7);
        var selectedTab = _mainViewModel.SelectedFriendEditViewModel;

        _mainViewModel.CloseFriendTabCommand.Execute(selectedTab);

        //Assert.Null(_mainViewModel.FriendEditViewModels.SingleOrDefault(s => s == selectedTab));
        Assert.Empty(_mainViewModel.FriendEditViewModels);
    }


    private class NavigationViewModelMock : INavigationViewModel
    {
        public bool HasLoaded { get; private set; }

        public void Load()
        {
            HasLoaded = true;
        }
    }
}
}
