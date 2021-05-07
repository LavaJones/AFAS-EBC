using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Extension methods for IList<User> lists.
/// </summary>
public static class IListOfUserExtensionMethods
{

    /// <summary>
    /// Filters a list of user instances for a specific userResourceId
    /// </summary>
    public static IQueryable<User> FilterForResourceId(this IList<User> users, Guid userResourceId)
    {

        return 
            (
                from user in users
                where
                    user.ResourceId == userResourceId
                select
                    user
            ).AsQueryable();

    }

}

