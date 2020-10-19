using FriendStorage.UI.View;
using System.Windows;
using FriendStorage.UI.Startup;
using DryIoc;

namespace FriendStorage.UI
{
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var container = new Bootstrapper();
        var builder = container.Bootstrap();

        var mainWindow = builder.Resolve<MainWindow>();
        mainWindow.Show();
    }
}
}
