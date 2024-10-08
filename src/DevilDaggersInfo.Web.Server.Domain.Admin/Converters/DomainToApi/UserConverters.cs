using DevilDaggersInfo.Web.ApiSpec.Admin.Users;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Entities;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;

public static class UserConverters
{
	public static GetUser ToAdminApi(this UserEntity user)
	{
		return new GetUser
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
			RegistrationDate = user.DateRegistered ?? DateTime.UnixEpoch,
		};

		static bool IsInRole(UserEntity user, string roleName)
		{
			return user.UserRoles?.Exists(ur => ur.Role?.Name == roleName) == true;
		}
	}
}
