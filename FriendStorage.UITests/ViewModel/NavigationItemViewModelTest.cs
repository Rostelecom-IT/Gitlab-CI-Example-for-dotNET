using FriendStorage.UI.ViewModel;
using Moq;
using Prism.Events;
using Xunit;
using OpenFriendEditViewEvent = FriendStorage.UI.Events.OpenFriendEditViewEvent;

namespace FriendStorage.UITests.ViewModel
{
public class NavigationItemViewModelTest
{
    [Fact]
    public void ShouldPublishOpenFriendEditViewEvent()
    {
        var friendId = 7;
        var eventMoq = new Mock<OpenFriendEditViewEvent>();
        var eventAggregatorMoq = new Mock<IEventAggregator>();
        eventAggregatorMoq
            .Setup(s =>s.GetEvent<OpenFriendEditViewEvent>())
            .Returns(eventMoq.Object);

        var viewModel = new NavigationItemViewModel(friendId, 
            "Thomas", eventAggregatorMoq.Object);
        viewModel.OpenFriendEditViewCommand.Execute(null);

        eventMoq.Verify(of => of.Publish(friendId), Times.Once);
    }
}
}
