using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Authorization
{
	public static class AuthorizationManager
	{
		public static readonly IReadOnlyDictionary<string, string> FolderToPolicyMapper = new Dictionary<string, string>()
		{
			{ "/Admin/AdminTests", Policies.AdminPolicy },
			{ "/Admin/AssetMods", Policies.AssetModsPolicy },
			{ "/Admin/CustomEntries", Policies.AdminPolicy },
			{ "/Admin/CustomLeaderboards", Policies.CustomLeaderboardsPolicy },
			{ "/Admin/Donations", Policies.DonationsPolicy },
			{ "/Admin/Players", Policies.PlayersPolicy },
			{ "/Admin/SpawnsetFiles", Policies.SpawnsetsPolicy },
			{ "/Admin/Titles", Policies.AdminPolicy },
		};

		public static async Task CreateRolesAndAdminUser(this IServiceProvider serviceProvider, string adminUserEmail)
		{
			RoleManager<IdentityRole>? roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			UserManager<IdentityUser>? userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

			foreach (string roleName in Policies.All)
			{
				bool roleExist = await roleManager.RoleExistsAsync(roleName);
				if (!roleExist)
					await roleManager.CreateAsync(new IdentityRole(roleName));
			}

			IdentityUser? admin = await userManager.FindByEmailAsync(adminUserEmail);
			if (admin != null)
			{
				foreach (string role in Policies.All)
					await userManager.AddToRoleAsync(admin, role);
			}
		}
	}
}
