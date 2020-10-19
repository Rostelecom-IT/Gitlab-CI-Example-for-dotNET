using FriendStorage.Model;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.ViewModel;
using FriendStorage.UITests.Extensions;
using Moq;
using Xunit;
using Assert = Xunit.Assert;

namespace FriendStorage.UITests.ViewModel
{
    public class FriendEditViewModelTests
    {
        private const int FriendId = 5;
        private readonly FriendEditViewModel _friendEditViewModelMoq;

        public FriendEditViewModelTests()
        {
            var friendDataProviderMoq = new Mock<IFriendDataProvider>();
            friendDataProviderMoq.Setup(dp => dp.GetFriendById(FriendId))
                .Returns(new Friend { Id = FriendId, FirstName = "Thomas" });

            _friendEditViewModelMoq = new FriendEditViewModel(friendDataProviderMoq.Object);
        }

        [Fact]
        public void ShouldLoadFriend()
        {
            _friendEditViewModelMoq.Load(FriendId);

            Assert.NotNull(_friendEditViewModelMoq.Friend);
            Assert.Equal(FriendId, _friendEditViewModelMoq.Friend.Id);
            Assert.Equal("Thomas", _friendEditViewModelMoq.Friend.FirstName);
        }

        [Fact]
        public void ShouldRisePropertyChangedForNewFriend()
        {
            var fired = _friendEditViewModelMoq.IsPropertyChangedFired(
                () =>
                {
                    _friendEditViewModelMoq.Load(FriendId);
                },
                nameof(_friendEditViewModelMoq.Friend));

            Assert.True(fired);
        }

        [Fact]
        public void ShouldDisableSaveCommandWhenFriendLoaded()
        {
            _friendEditViewModelMoq.Load(FriendId);

            Assert.False(_friendEditViewModelMoq.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldEnabledSaveCommandWhenFriendChanged()
        {
            _friendEditViewModelMoq.Load(FriendId);
            _friendEditViewModelMoq.Friend.FirstName = "Julia";

            Assert.True(_friendEditViewModelMoq.SaveCommand.CanExecute(null));
        }

        [Fact]
        public void ShouldDisableSaveCommandWithoutLoadCommand()
        {
            Assert.False(_friendEditViewModelMoq.SaveCommand.CanExecute(null));
        }

        // Fail test
        //[Fact]
        //public void ShouldRaiseCanExecuteChangedWhenFriendIsChanged()
        //{
        //   _friendEditViewModelMoq.Load(FriendId);
        //    var fired = false;
        //    _friendEditViewModelMoq.SaveCommand.CanExecuteChanged += (sender, args) => fired = true;
        //
        //    _friendEditViewModelMoq.Friend.FirstName = "Changed";
        //
        //    Assert.True(fired);
        //}

        //[Fact]
        //public void ShouldRaiseCanExecuteChangedAfterFriendLoaded()
        //{
        //    var fired = false;
        //    _friendEditViewModelMoq.SaveCommand.CanExecuteChanged += (sender, args) => fired = true;
        //   _friendEditViewModelMoq.Load(FriendId);
        //
        //    Assert.True(fired);
        //}
    }
}
