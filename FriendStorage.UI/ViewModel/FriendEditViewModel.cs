using FriendStorage.UI.DataProvider;
using FriendStorage.UI.Wrapper;
using Prism.Commands;
using System.Windows.Input;

namespace FriendStorage.UI.ViewModel
{
    public interface IFriendEditViewModel
    {
        void Load(int id);
        FriendWrapper Friend { get; }
    }

    public class FriendEditViewModel : ViewModelBase, IFriendEditViewModel
    {
        private readonly IFriendDataProvider _friendDataProvider;
        private FriendWrapper _friend;

        public FriendEditViewModel(IFriendDataProvider friendDataProvider)
        {
            _friendDataProvider = friendDataProvider;
            SaveCommand = new DelegateCommand(OnSaveExecuted, OnSaveCanExecuted);
        }

        public FriendWrapper Friend
        {
            get => _friend;
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }
        public ICommand SaveCommand { get; }

        public void Load(int friendId)
        {
            var friend = _friendDataProvider.GetFriendById(friendId);
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged +=(sender, args) =>    
            {
                //if (args.PropertyName == nameof(Friend.IsChanged))
                    ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
            };
            ((DelegateCommand) SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnSaveCanExecuted()
        {
            return Friend?.IsChanged ?? false;
        }
        private void OnSaveExecuted()
        {
            throw new System.NotImplementedException();
        }
    }
}
