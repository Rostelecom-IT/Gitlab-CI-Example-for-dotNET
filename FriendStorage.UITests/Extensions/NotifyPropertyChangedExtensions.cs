using System;
using System.ComponentModel;

namespace FriendStorage.UITests.Extensions
{
public static class NotifyPropertyChangedExtensions
{
    public static bool IsPropertyChangedFired(
        this INotifyPropertyChanged notifyPropertyChanged, 
        Action action,
        string propertyName)
    {
        var fire = false;
        notifyPropertyChanged.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == propertyName)
                fire = true;
        };

        action();

        return fire;
    }
}
}
