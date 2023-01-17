using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUF.Shared.Authorization;

namespace TUF.Shared.Authorization;
  



 



public   class TUFPermissions
{
	private static readonly TUFPermission[] _all = new TUFPermission[]
	{
		new("View Dashboard", TUFAction.View, TUFResource.Dashboard),
		new("View Hangfire", TUFAction.View, TUFResource.Hangfire),
		new("View Users", TUFAction.View, TUFResource.Users),
		new("Search Users", TUFAction.Search, TUFResource.Users),
		new("Create Users", TUFAction.Create, TUFResource.Users),
		new("Update Users", TUFAction.Update, TUFResource.Users),
		new("Delete Users", TUFAction.Delete, TUFResource.Users),
		new("Export Users", TUFAction.Export, TUFResource.Users),
		new("View UserRoles", TUFAction.View, TUFResource.UserRoles),
		new("Update UserRoles", TUFAction.Update, TUFResource.UserRoles),
		new("View Roles", TUFAction.View, TUFResource.Roles),
		new("Create Roles", TUFAction.Create, TUFResource.Roles),
		new("Update Roles", TUFAction.Update, TUFResource.Roles),
		new("Delete Roles", TUFAction.Delete, TUFResource.Roles),
		new("View RoleClaims", TUFAction.View, TUFResource.RoleClaims),
		new("Update RoleClaims", TUFAction.Update, TUFResource.RoleClaims),
		new("View Products", TUFAction.View, TUFResource.Products, IsBasic: true),
		new("Search Products", TUFAction.Search, TUFResource.Products, IsBasic: true),
		new("Create Products", TUFAction.Create, TUFResource.Products),
		new("Update Products", TUFAction.Update, TUFResource.Products),
		new("Delete Products", TUFAction.Delete, TUFResource.Products),
		new("Export Products", TUFAction.Export, TUFResource.Products),
		new("View Brands", TUFAction.View, TUFResource.Brands, IsBasic: true),
		new("Search Brands", TUFAction.Search, TUFResource.Brands, IsBasic: true),
		new("Create Brands", TUFAction.Create, TUFResource.Brands),
		new("Update Brands", TUFAction.Update, TUFResource.Brands),
		new("Delete Brands", TUFAction.Delete, TUFResource.Brands),
		new("Generate Brands", TUFAction.Generate, TUFResource.Brands),
		new("Clean Brands", TUFAction.Clean, TUFResource.Brands),
		new("View Tenants", TUFAction.View, TUFResource.Tenants, IsRoot: true),
		new("Create Tenants", TUFAction.Create, TUFResource.Tenants, IsRoot: true),
		new("Update Tenants", TUFAction.Update, TUFResource.Tenants, IsRoot: true),
		new("Upgrade Tenant Subscription", TUFAction.UpgradeSubscription, TUFResource.Tenants, IsRoot: true)
	};

	public static IReadOnlyList<TUFPermission> All { get; } = new ReadOnlyCollection<TUFPermission>(_all);
	public static IReadOnlyList<TUFPermission> Root { get; } = new ReadOnlyCollection<TUFPermission>(_all.Where(p => p.IsRoot).ToArray());
	public static IReadOnlyList<TUFPermission> Admin { get; } = new ReadOnlyCollection<TUFPermission>(_all.Where(p => !p.IsRoot).ToArray());
	public static IReadOnlyList<TUFPermission> Basic { get; } = new ReadOnlyCollection<TUFPermission>(_all.Where(p => p.IsBasic).ToArray());
}



