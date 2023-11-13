using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Forces.Shared.Constants.Notification
{
    public static class NotificationsEntities
    {

        public static class Items
        {
            public const string OnAdd = "Notifications.Items.Add";
            public const string OnUpdate = "Notifications.Items.Edit";
            public const string OnDelete = "Notifications.Items.Delete";
        }
        public static class Requests
        {
            public const string OnAdd = "Notifications.Requests.Add";
            public const string OnUpdate = "Notifications.Requests.Edit";
            public const string OnDelete = "Notifications.Requests.Delete";
        }
        public static class VoteCodes
        {
            public const string OnAdd = "Notifications.VoteCodes.Add";
            public const string OnUpdate = "Notifications.VoteCodes.Edit";
            public const string OnDelete = "Notifications.VoteCodes.Delete";
        }
        public static class VodeCodesLogs
        {
            public const string OnAdd = "Notifications.VodeCodesLogs.Add";
            public const string OnUpdate = "Notifications.VodeCodesLogs.Edit";
            public const string OnDelete = "Notifications.VodeCodesLogs.Delete";
        }
        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredEntities()
        {
            var Entities = new List<string>();
            foreach (var prop in typeof(NotificationsEntities).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    Entities.Add(propertyValue.ToString());
            }
            return Entities;
        }
    }
}
