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
}
