using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using AdminApi = DevilDaggersInfo.Api.Admin.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Admin;

public static class CustomLeaderboardConverters
{
	public static CustomLeaderboardDaggers ToDomain(this AdminApi.AddCustomLeaderboardDaggers addCustomLeaderboardDaggers) => new()
	{
		Bronze = addCustomLeaderboardDaggers.Bronze.To10thMilliTime(),
		Silver = addCustomLeaderboardDaggers.Silver.To10thMilliTime(),
		Golden = addCustomLeaderboardDaggers.Golden.To10thMilliTime(),
		Devil = addCustomLeaderboardDaggers.Devil.To10thMilliTime(),
		Leviathan = addCustomLeaderboardDaggers.Leviathan.To10thMilliTime(),
	};

	public static CustomLeaderboardCategory ToDomain(this AdminApi.CustomLeaderboardCategory customLeaderboardCategory) => customLeaderboardCategory switch
	{
		AdminApi.CustomLeaderboardCategory.Survival => CustomLeaderboardCategory.Survival,
		AdminApi.CustomLeaderboardCategory.TimeAttack => CustomLeaderboardCategory.TimeAttack,
		AdminApi.CustomLeaderboardCategory.Speedrun => CustomLeaderboardCategory.Speedrun,
		AdminApi.CustomLeaderboardCategory.Race => CustomLeaderboardCategory.Race,
		AdminApi.CustomLeaderboardCategory.Pacifist => CustomLeaderboardCategory.Pacifist,
		AdminApi.CustomLeaderboardCategory.RaceNoShooting => CustomLeaderboardCategory.RaceNoShooting,
		_ => throw new InvalidEnumConversionException(customLeaderboardCategory),
	};
}
