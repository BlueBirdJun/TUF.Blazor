using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUF.Shared.Identitys
{
    public static class TUFAction
    { 
        public const string View = nameof(View);
        public const string Search = nameof(Search);
        public const string Create = nameof(Create);
        public const string Update = nameof(Update);
        public const string Delete = nameof(Delete);
        public const string Export = nameof(Export);
        public const string Generate = nameof(Generate);
        public const string Clean = nameof(Clean);
        public const string UpgradeSubscription = nameof(UpgradeSubscription);

    }

    public static class TUFResource
    {
        public const string Tenants = nameof(Tenants);
        public const string Dashboard = nameof(Dashboard);
        public const string Hangfire = nameof(Hangfire);
        public const string Users = nameof(Users);
        public const string UserRoles = nameof(UserRoles);
        public const string Roles = nameof(Roles);
        public const string RoleClaims = nameof(RoleClaims);
        public const string Products = nameof(Products);
        public const string Brands = nameof(Brands);
    }
}
