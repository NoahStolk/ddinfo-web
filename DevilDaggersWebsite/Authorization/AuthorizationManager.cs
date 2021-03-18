using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Authorization
{
	public static class AuthorizationManager
	{
		public const string AdminPolicy = nameof(AdminPolicy);
		public const string AdminRole = nameof(AdminRole);

		public const string AssetModsPolicy = nameof(AssetModsPolicy);
		public const string AssetModsRole = nameof(AssetModsRole);

		public const string CustomLeaderboardsPolicy = nameof(CustomLeaderboardsPolicy);
		public const string CustomLeaderboardsRole = nameof(CustomLeaderboardsRole);

		public const string DonationsPolicy = nameof(DonationsPolicy);
		public const string DonationsRole = nameof(DonationsRole);

		public const string PlayersPolicy = nameof(PlayersPolicy);
		public const string PlayersRole = nameof(PlayersRole);

		public const string SpawnsetsPolicy = nameof(SpawnsetsPolicy);
		public const string SpawnsetsRole = nameof(SpawnsetsRole);

		public static readonly Dictionary<string, string> PolicyToRoleMapper = new()
		{
			{ AdminPolicy, AdminRole },
			{ AssetModsPolicy, AssetModsRole },
			{ CustomLeaderboardsPolicy, CustomLeaderboardsRole },
			{ DonationsPolicy, DonationsRole },
			{ PlayersPolicy, PlayersRole },
			{ SpawnsetsPolicy, SpawnsetsRole },
		};

		public static readonly Dictionary<string, string> FolderToPolicyMapper = new()
		{
			{ "/Admin/AdminTests", AdminPolicy },
			{ "/Admin/AssetMods", AssetModsPolicy },
			{ "/Admin/CustomEntries", AdminPolicy },
			{ "/Admin/CustomLeaderboards", CustomLeaderboardsPolicy },
			{ "/Admin/Donations", DonationsPolicy },
			{ "/Admin/Players", PlayersPolicy },
			{ "/Admin/SpawnsetFiles", SpawnsetsPolicy },
			{ "/Admin/Titles", AdminPolicy },
		};

		public static async Task CreateRolesAndAdminUser(this IServiceProvider serviceProvider, string adminUserEmail)
		{
			RoleManager<IdentityRole>? roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			UserManager<IdentityUser>? userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

			foreach (string? roleName in PolicyToRoleMapper.Select(kvp => kvp.Value).ToArray())
			{
				bool roleExist = await roleManager.RoleExistsAsync(roleName);
				if (!roleExist)
					await roleManager.CreateAsync(new IdentityRole(roleName));
			}

			IdentityUser? admin = await userManager.FindByEmailAsync(adminUserEmail);
			if (admin != null)
			{
				foreach (string role in PolicyToRoleMapper.Values)
					await userManager.AddToRoleAsync(admin, role);
			}
		}
	}
}
