using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Prism.Events;
using Xunit;

namespace FriendStorage.UITests.ViewModel
{
public class NavigationViewModelTests
{
    private readonly NavigationViewModel _viewModel;

    public NavigationViewModelTests()
    {
        // Mocking
        var eventAggregatorMoq = new Mock<IEventAggregator>();
        var moq = new Mock<INavigationDataProvider>();
        moq.Setup(s => s.GetAllFriends()).Returns(() => new List<LookupItem>
        {
            new LookupItem { Id = 1, DisplayMember = "Julia" },
            new LookupItem { Id = 2, DisplayMember = "Thomas" }
        });
        _viewModel = new NavigationViewModel(moq.Object, eventAggregatorMoq.Object);
    }

    [Fact]
    public void ShouldLoadFriends()
    {
        _viewModel.Load();

        Assert.Equal(2, _viewModel.Friends.Count);

        var friend = _viewModel.Friends.SingleOrDefault(s => s.Id == 1);
        Assert.NotNull(friend);
        Assert.Equal("Julia", friend.DisplayMember);

        friend = _viewModel.Friends.SingleOrDefault(s => s.Id == 2);
        Assert.NotNull(friend);
        Assert.Equal("Thomas", friend.DisplayMember);
    }

    [Fact]
    public void ShouldLoadFriendsOnlyOnce()
    {
        _viewModel.Load();
        _viewModel.Load();

        Assert.Equal(2, _viewModel.Friends.Count);
    }

    private class NavigationDataProviderMock : INavigationDataProvider
    {
        public IEnumerable<LookupItem> GetAllFriends()
        {
            yield return new LookupItem { Id = 1, DisplayMember = "Julia" };
            yield return new LookupItem { Id = 2, DisplayMember = "Thomas" };
        }
    }
}
}
