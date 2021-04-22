using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeFinderWebApi.Exchange.Management
{
    public static class RequestInfoManager
    {
        public static UserActionExtension Action { get; private set; } = new UserActionExtension();

        public static bool IsUserSet { get { return Action.UserId > 0 || Action.User != null; } }
        public static bool IsEndpointSet { get { return Action.Endpoint != null; } }
        public static bool IsCompleted { get { return !String.IsNullOrEmpty(Action.Description) && Action.UserId > 0; } }
        public static bool IsObjectReferenceSet { get { return !String.IsNullOrEmpty(Action.RefObjectId) || Action.RefObject != null || !String.IsNullOrEmpty(Action.RefObjectName); } }

        public static string Error { get; set; }

        public static Func<int> LogUserAction { get; set; } = new Func<int>(() => { throw new ArgumentException("Method for logging UserAction has not been set."); });

        public static void Reset()
        {
            Action = new UserActionExtension();
        }
    }
}
