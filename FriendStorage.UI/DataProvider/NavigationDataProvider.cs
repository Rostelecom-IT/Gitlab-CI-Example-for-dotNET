using System;
using System.Collections.Generic;
using FriendStorage.DataAccess;
using FriendStorage.Model;

namespace FriendStorage.UI.DataProvider
{
public class NavigationDataProvider : INavigationDataProvider
{
    private readonly Func<IDataService> _dataServiceCreator;

    public NavigationDataProvider(Func<IDataService> dataServiceCreator)
    {
        _dataServiceCreator = dataServiceCreator;
    }
    public IEnumerable<LookupItem> GetAllFriends()
    {
        using (var ctx = _dataServiceCreator())
        {
            return ctx.GetAllFriends();
        }
    }
}
}
