using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Authorization
{
	public static class AuthorizationManager
	{
		private const string _adminUserEmail = "noah.stolk@gmail.com";

		public static readonly IReadOnlyDictionary<string, string> FolderToPolicyMapper = new Dictionary<string, string>()
		{
			{ "/Admin/AdminTests", Policies.AdminPolicy },
			{ "/Admin/AssetMods", Policies.AssetModsPolicy },
			{ "/Admin/CustomLeaderboards", Policies.CustomLeaderboardsPolicy },
			{ "/Admin/Players", Policies.PlayersPolicy },
			{ "/Admin/SpawnsetFiles", Policies.SpawnsetsPolicy },
		};

		public static async Task CreateRolesAndAdminUser(this IServiceProvider serviceProvider)
		{
			RoleManager<IdentityRole>? roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			UserManager<IdentityUser>? userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

			foreach (string policy in Policies.All)
			{
				bool policyExists = await roleManager.RoleExistsAsync(policy);
				if (!policyExists)
					await roleManager.CreateAsync(new IdentityRole(policy));
			}

			IdentityUser? admin = await userManager.FindByEmailAsync(_adminUserEmail);
			if (admin != null)
			{
				foreach (string policy in Policies.All)
					await userManager.AddToRoleAsync(admin, policy);
			}
		}
	}
}
