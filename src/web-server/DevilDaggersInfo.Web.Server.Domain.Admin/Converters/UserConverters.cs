using DevilDaggersInfo.Api.Admin.Users;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Entities;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters;

public static class UserConverters
{
	public static GetUser ToGetUser(this UserEntity user) => new()
	{
		Id = user.Id,
		Name = user.Name,
		IsAdmin = IsInRole(user, Roles.Admin),
		IsCustomLeaderboardsMaintainer = IsInRole(user, Roles.CustomLeaderboards),
		IsModsMaintainer = IsInRole(user, Roles.Mods),
		IsPlayersMaintainer = IsInRole(user, Roles.Players),
		IsSpawnsetsMaintainer = IsInRole(user, Roles.Spawnsets),
		PlayerId = user.PlayerId,
		PlayerName = user.Player?.PlayerName ?? string.Empty,
	};

	private static bool IsInRole(UserEntity user, string roleName)
		=> user.UserRoles?.Any(ur => ur.Role?.Name == roleName) == true;
}
