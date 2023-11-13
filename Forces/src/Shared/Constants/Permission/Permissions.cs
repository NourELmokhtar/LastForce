using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Forces.Shared.Constants.Permission
{
    public static class Permissions
    {
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
            public const string Export = "Permissions.Products.Export";
            public const string Search = "Permissions.Products.Search";
        }
        public static class Building
        {
            public const string View = "Permissions.Building.View";
            public const string Create = "Permissions.Building.Create";
            public const string Edit = "Permissions.Building.Edit";
            public const string Delete = "Permissions.Building.Delete";
            public const string Export = "Permissions.Building.Export";
            public const string Search = "Permissions.Building.Search";
        }
        public static class Room
        {
            public const string View = "Permissions.Room.View";
            public const string Create = "Permissions.Room.Create";
            public const string Edit = "Permissions.Room.Edit";
            public const string Delete = "Permissions.Room.Delete";
            public const string Export = "Permissions.Room.Export";
            public const string Search = "Permissions.Room.Search";
        }
        public static class House
        {
            public const string View = "Permissions.House.View";
            public const string Create = "Permissions.House.Create";
            public const string Edit = "Permissions.House.Edit";
            public const string Delete = "Permissions.House.Delete";
            public const string Export = "Permissions.House.Export";
            public const string Search = "Permissions.House.Search";
        }
        public static class Person
        {
            public const string View = "Permissions.Person.View";
            public const string Create = "Permissions.Person.Create";
            public const string Edit = "Permissions.Person.Edit";
            public const string Delete = "Permissions.Person.Delete";
            public const string Export = "Permissions.Person.Export";
            public const string Search = "Permissions.Person.Search";
        }
        public static class Rooms
        {
            public const string View = "Permissions.Rooms.View";
            public const string Create = "Permissions.Rooms.Create";
            public const string Edit = "Permissions.Rooms.Edit";
            public const string Delete = "Permissions.Rooms.Delete";
            public const string Export = "Permissions.Rooms.Export";
            public const string Search = "Permissions.Rooms.Search";
        }
        public static class Office
        {
            public const string View = "Permissions.Office.View";
            public const string Create = "Permissions.Office.Create";
            public const string Edit = "Permissions.Office.Edit";
            public const string Delete = "Permissions.Office.Delete";
            public const string Export = "Permissions.Office.Export";
            public const string Search = "Permissions.Office.Search";
        }
        public static class Inventory
        {
            public const string View = "Permissions.Inventory.View";
            public const string Create = "Permissions.Inventory.Create";
            public const string Edit = "Permissions.Inventory.Edit";
            public const string Delete = "Permissions.Inventory.Delete";
            public const string Export = "Permissions.Inventory.Export";
            public const string Search = "Permissions.Inventory.Search";
        }
        public static class InventoryItems
        {
            public const string View = "Permissions.InventoryItems.View";
            public const string Create = "Permissions.InventoryItems.Create";
            public const string Edit = "Permissions.InventoryItems.Edit";
            public const string Delete = "Permissions.InventoryItems.Delete";
            public const string Export = "Permissions.InventoryItems.Export";
            public const string Search = "Permissions.InventoryItems.Search";
        }
        public static class InventoryItemsBridge
        {
            public const string View = "Permissions.InventoryItemsBridge.View";
            public const string Create = "Permissions.InventoryItemsBridge.Create";
            public const string Edit = "Permissions.InventoryItemsBridge.Edit";
            public const string Delete = "Permissions.InventoryItemsBridge.Delete";
            public const string Export = "Permissions.InventoryItemsBridge.Export";
            public const string Search = "Permissions.InventoryItemsBridge.Search";
        }

        public static class Brands
        {
            public const string View = "Permissions.Brands.View";
            public const string Create = "Permissions.Brands.Create";
            public const string Edit = "Permissions.Brands.Edit";
            public const string Delete = "Permissions.Brands.Delete";
            public const string Export = "Permissions.Brands.Export";
            public const string Search = "Permissions.Brands.Search";
        }

        public static class Documents
        {
            public const string View = "Permissions.Documents.View";
            public const string Create = "Permissions.Documents.Create";
            public const string Edit = "Permissions.Documents.Edit";
            public const string Delete = "Permissions.Documents.Delete";
            public const string Search = "Permissions.Documents.Search";
        }

        public static class DocumentTypes
        {
            public const string View = "Permissions.DocumentTypes.View";
            public const string Create = "Permissions.DocumentTypes.Create";
            public const string Edit = "Permissions.DocumentTypes.Edit";
            public const string Delete = "Permissions.DocumentTypes.Delete";
            public const string Export = "Permissions.DocumentTypes.Export";
            public const string Search = "Permissions.DocumentTypes.Search";
        }

        public static class DocumentExtendedAttributes
        {
            public const string View = "Permissions.DocumentExtendedAttributes.View";
            public const string Create = "Permissions.DocumentExtendedAttributes.Create";
            public const string Edit = "Permissions.DocumentExtendedAttributes.Edit";
            public const string Delete = "Permissions.DocumentExtendedAttributes.Delete";
            public const string Export = "Permissions.DocumentExtendedAttributes.Export";
            public const string Search = "Permissions.DocumentExtendedAttributes.Search";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
            public const string Export = "Permissions.Users.Export";
            public const string Search = "Permissions.Users.Search";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string Search = "Permissions.Roles.Search";
        }

        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
            public const string Search = "Permissions.RoleClaims.Search";
        }

        public static class Communication
        {
            public const string Chat = "Permissions.Communication.Chat";
        }

        public static class Preferences
        {
            public const string ChangeLanguage = "Permissions.Preferences.ChangeLanguage";

            //TODO - add permissions
        }

        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
        }

        public static class Hangfire
        {
            public const string View = "Permissions.Hangfire.View";
        }

        public static class AuditTrails
        {
            public const string View = "Permissions.AuditTrails.View";
            public const string Export = "Permissions.AuditTrails.Export";
            public const string Search = "Permissions.AuditTrails.Search";
        }
        public static class BasicInformations
        {
            public const string ViewForces = "Permissions.BasicInformations.ViewForces";
            public const string CreateForces = "Permissions.BasicInformations.CreateForces";
            public const string EditForces = "Permissions.BasicInformations.EditForces";
            public const string DeleteForces = "Permissions.BasicInformations.DeleteForces";
            public const string SearchForces = "Permissions.BasicInformations.SearchForces";
            public const string ViewBases = "Permissions.BasicInformations.ViewBases";
            public const string CreateBases = "Permissions.BasicInformations.CreateBases";
            public const string EditBases = "Permissions.BasicInformations.EditBases";
            public const string DeleteBases = "Permissions.BasicInformations.DeleteBases";
            public const string SearchBases = "Permissions.BasicInformations.SearchBases";
            public const string ViewBasesSection = "Permissions.BasicInformations.ViewBasesSection";
            public const string CreateBasesSection = "Permissions.BasicInformations.CreateBasesSection";
            public const string EditBasesSection = "Permissions.BasicInformations.EditBasesSection";
            public const string DeleteBasesSection = "Permissions.BasicInformations.DeleteBasesSection";
            public const string SearchBasesSection = "Permissions.BasicInformations.SearchBasesSection";
            public const string ViewDepartments = "Permissions.BasicInformations.ViewHQDepartments";
            public const string CreateDepartments = "Permissions.BasicInformations.CreateHQDepartments";
            public const string EditDepartments = "Permissions.BasicInformations.EditHQDepartments";
            public const string DeleteDepartments = "Permissions.BasicInformations.DeleteHQDepartments";
            public const string SearchDepartments = "Permissions.BasicInformations.SearchHQDepartments";

        }
        public static class HQManagement
        {
            public const string ViewDepartments = "Permissions.HQManagement.ViewDepartments";
            public const string CreateDepartments = "Permissions.HQManagement.CreateDepartments";
            public const string EditDepartments = "Permissions.HQManagement.EditDepartments";
            public const string DeleteDepartments = "Permissions.HQManagement.DeleteDepartments";
            public const string SearchDepartments = "Permissions.HQManagement.SearchDepartments";

        }
        public static class DepoManagement
        {
            public const string ViewDepartments = "Permissions.DepoManagement.ViewDepartments";
            public const string CreateDepartments = "Permissions.DepoManagement.CreateDepartments";
            public const string EditDepartments = "Permissions.DepoManagement.EditDepartments";
            public const string DeleteDepartments = "Permissions.DepoManagement.DeleteDepartments";
            public const string SearchDepartments = "Permissions.DepoManagement.SearchDepartments";

        }
        public static class VoteCodes
        {
            public const string View = "Permissions.VoteCodes.View";
            public const string Create = "Permissions.VoteCodes.Create";
            public const string Edit = "Permissions.VoteCodes.Edit";
            public const string Assign = "Permissions.VoteCodes.Assign";
            public const string Delete = "Permissions.VoteCodes.Delete";
            public const string Export = "Permissions.VoteCodes.Export";
            public const string Search = "Permissions.VoteCodes.Search";
        }
        public static class VoteCodeController
        {
            public const string ViewTransactions = "Permissions.VoteCodeController.ViewTransactions";
            public const string CreateTransactions = "Permissions.VoteCodeController.CreateTransactions";
            public const string ViewCridet = "Permissions.VoteCodeController.ViewCridet";
            public const string ApproveRequests = "Permissions.VoteCodeController.ApproveRequests";
            public const string ViewDashboard = "Permissions.VoteCodeController.ViewDashboard";

        }
        public static class Items
        {
            public const string View = "Permissions.Items.View";
            public const string Create = "Permissions.Items.Create";
            public const string Edit = "Permissions.Items.Edit";
            public const string Delete = "Permissions.Items.Delete";
            public const string Export = "Permissions.Items.Export";
            public const string Search = "Permissions.Items.Search";
        }
        public static class MeasureUnits
        {
            public const string View = "Permissions.MeasureUnits.View";
            public const string Create = "Permissions.MeasureUnits.Create";
            public const string Edit = "Permissions.MeasureUnits.Edit";
            public const string Delete = "Permissions.MeasureUnits.Delete";
            public const string Search = "Permissions.MeasureUnits.Search";
        }
        public static class MPR
        {
            public const string View = "Permissions.MPR.View";
            public const string Create = "Permissions.MPR.Create";
            public const string Search = "Permissions.MPR.Search";
        }
        public static class BOD
        {
            public const string View = "Permissions.BOD.View";
            public const string Create = "Permissions.BOD.Create";
            public const string Search = "Permissions.BOD.Search";

        }
        public static class Cash
        {
            public const string View = "Permissions.Cash.View";
            public const string Create = "Permissions.Cash.Create";
            public const string Search = "Permissions.Cash.Search";
        }
        public static class PersonalServices
        {
            public const string View = "Permissions.PersonalServices.View";
            public const string Create = "Permissions.PersonalServices.Create";
            public const string Edit = "Permissions.PersonalServices.Edit";
            public const string Search = "Permissions.PersonalServices.Search";
        }
        public static class PersonalServicesItems
        {
            public const string View = "Permissions.PersonalServicesItems.View";
            public const string Create = "Permissions.PersonalServicesItems.Create";
            public const string Edit = "Permissions.PersonalServicesItems.Edit";
            public const string Delete = "Permissions.PersonalServicesItems.Delete";
            public const string Search = "Permissions.PersonalServicesItems.Search";
        }
        public static class Tailer
        {
            public const string View = "Permissions.Tailer.View";
            public const string Create = "Permissions.Tailer.Create";
            public const string Edit = "Permissions.Tailer.Edit";
            public const string Delete = "Permissions.Tailer.Delete";
            public const string Search = "Permissions.Tailer.Search";
        }
        public static class PersonalItemsOperations
        {
            public const string View = "Permissions.PersonalItemsOperations.View";
            public const string Create = "Permissions.PersonalItemsOperations.Create";
            public const string Edit = "Permissions.PersonalItemsOperations.Edit";
            public const string Delete = "Permissions.PersonalItemsOperations.Delete";
            public const string Search = "Permissions.PersonalItemsOperations.Search";
            public const string Exceptions = "Permissions.PersonalItemsOperations.Exceptions";
            public const string Reports = "Permissions.PersonalItemsOperations.Reports";
            public const string Export = "Permissions.PersonalItemsOperations.Export";
        }
        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permssions = new List<string>();
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permssions.Add(propertyValue.ToString());
            }
            return permssions;
        }
    }
}