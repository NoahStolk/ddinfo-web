using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Api.Main.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Domain.Main.Converters.DomainToApi;

public static class CustomLeaderboardConverters
{
	public static MainApi.CustomLeaderboardRankSorting ToMainApi(this CustomLeaderboardRankSorting rankSorting) => rankSorting switch
	{
		CustomLeaderboardRankSorting.TimeDesc => MainApi.CustomLeaderboardRankSorting.TimeDesc,
		CustomLeaderboardRankSorting.TimeAsc => MainApi.CustomLeaderboardRankSorting.TimeAsc,
		CustomLeaderboardRankSorting.GemsDesc => MainApi.CustomLeaderboardRankSorting.GemsDesc,
		CustomLeaderboardRankSorting.KillsDesc => MainApi.CustomLeaderboardRankSorting.KillsDesc,
		CustomLeaderboardRankSorting.HomingDesc => MainApi.CustomLeaderboardRankSorting.HomingDesc,
		_ => throw new UnreachableException(),
	};
}
