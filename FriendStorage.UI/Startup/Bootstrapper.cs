using DryIoc;
using FriendStorage.DataAccess;
using FriendStorage.UI.DataProvider;
using FriendStorage.UI.View;
using FriendStorage.UI.ViewModel;
using Prism.Events;

namespace FriendStorage.UI.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var container = new Container();

            container.Register<MainWindow>();
            container.Register<MainViewModel>();
            container.Register<INavigationViewModel, NavigationViewModel>();
            container.Register<INavigationDataProvider, NavigationDataProvider>();
            container.Register<IFriendEditViewModel, FriendEditViewModel>();
            container.Register<IDataService, FileDataService>(setup: Setup.With(allowDisposableTransient: true));

            container.Register<IEventAggregator, EventAggregator>(Reuse.Singleton);
            container.Register<IFriendDataProvider, FriendDataProvider>();

            return container;
        }
    }
}
