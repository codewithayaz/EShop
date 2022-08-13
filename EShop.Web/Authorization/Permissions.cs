using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EShop.Web.Authorization
{
    public static class Actions
    {
        public const string Create = nameof(Create);
        public const string Read = nameof(Read);
        public const string Update = nameof(Update);
        public const string Delete = nameof(Delete);
        public const string Manage = nameof(Manage);
    }

    public static class Permissions
    {
        public static class Role
        {
            [Description("Create a new Role")]
            public const string Create = nameof(Role) + "." + nameof(Actions.Create);
            [Description("Read roles data (permissions, etc.")]
            public const string Read = nameof(Role) + "." + nameof(Actions.Read);
            [Description("Edit existing Roles")]
            public const string Update = nameof(Role) + "." + nameof(Actions.Update);
            [Description("Delete any Role")]
            public const string Delete = nameof(Role) + "." + nameof(Actions.Delete);
            [Description("Manage any Role Permissions")]
            public const string Manage = nameof(Role) + "." + nameof(Actions.Manage);
        }
        public static class User
        {
            [Description("Create a new User")]
            public const string Create = nameof(User) + "." + nameof(Actions.Create);
            [Description("Read Users data (Names, Emails, Phone Numbers, etc.)")]
            public const string Read = nameof(User) + "." + nameof(Actions.Read);
            [Description("Edit existing users")]
            public const string Update = nameof(User) + "." + nameof(Actions.Update);
            [Description("Delete any user")]
            public const string Delete = nameof(User) + "." + nameof(Actions.Delete);
        }

        public static class Product
        {
            [Description("Create a new Product")]
            public const string Create = nameof(Product) + "." + nameof(Actions.Create);
            [Description("Read Products")]
            public const string Read = nameof(Product) + "." + nameof(Actions.Read);
            [Description("Edit existing Products")]
            public const string Update = nameof(Product) + "." + nameof(Actions.Update);
            [Description("Delete any Product")]
            public const string Delete = nameof(Product) + "." + nameof(Actions.Delete);
        }

        public static class Category
        {
            [Description("Create a new Category")]
            public const string Create = nameof(Category) + "." + nameof(Actions.Create);
            [Description("Read Categories")]
            public const string Read = nameof(Category) + "." + nameof(Actions.Read);
            [Description("Edit existing Categories")]
            public const string Update = nameof(Category) + "." + nameof(Actions.Update);
            [Description("Delete any Category")]
            public const string Delete = nameof(Category) + "." + nameof(Actions.Delete);
        }

        public static class Promotion
        {
            [Description("Create a new Promotion")]
            public const string Create = nameof(Promotion) + "." + nameof(Actions.Create);
            [Description("Read Promotions")]
            public const string Read = nameof(Promotion) + "." + nameof(Actions.Read);
            [Description("Edit existing Promotions")]
            public const string Update = nameof(Promotion) + "." + nameof(Actions.Update);
            [Description("Delete any Promotion")]
            public const string Delete = nameof(Promotion) + "." + nameof(Actions.Delete);
            [Description("Manage promotion products")]
            public const string Manage = nameof(Promotion) + "." + nameof(Actions.Manage);
        }

        public static class Invoice
        {
            [Description("Create a new Invoice")]
            public const string Create = nameof(Invoice) + "." + nameof(Actions.Create);
            [Description("Read Invoices")]
            public const string Read = nameof(Invoice) + "." + nameof(Actions.Read);
        }

    }
}