using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using System.Diagnostics;
using MainApi = DevilDaggersInfo.Api.Main.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Main;

public static class CustomLeaderboardConverters
{
	public static CustomLeaderboardSorting ToDomain(this MainApi.CustomLeaderboardSorting customLeaderboardSorting) => customLeaderboardSorting switch
	{
		MainApi.CustomLeaderboardSorting.SpawnsetName => CustomLeaderboardSorting.SpawnsetName,
		MainApi.CustomLeaderboardSorting.AuthorName => CustomLeaderboardSorting.AuthorName,
		MainApi.CustomLeaderboardSorting.TimeBronze => CustomLeaderboardSorting.TimeBronze,
		MainApi.CustomLeaderboardSorting.TimeSilver => CustomLeaderboardSorting.TimeSilver,
		MainApi.CustomLeaderboardSorting.TimeGolden => CustomLeaderboardSorting.TimeGolden,
		MainApi.CustomLeaderboardSorting.TimeDevil => CustomLeaderboardSorting.TimeDevil,
		MainApi.CustomLeaderboardSorting.TimeLeviathan => CustomLeaderboardSorting.TimeLeviathan,
		MainApi.CustomLeaderboardSorting.DateCreated => CustomLeaderboardSorting.DateCreated,
		MainApi.CustomLeaderboardSorting.DateLastPlayed => CustomLeaderboardSorting.DateLastPlayed,
		MainApi.CustomLeaderboardSorting.Submits => CustomLeaderboardSorting.Submits,
		MainApi.CustomLeaderboardSorting.Players => CustomLeaderboardSorting.Players,
		MainApi.CustomLeaderboardSorting.TopPlayer => CustomLeaderboardSorting.TopPlayer,
		MainApi.CustomLeaderboardSorting.WorldRecord => CustomLeaderboardSorting.WorldRecord,
		_ => throw new UnreachableException(),
	};

	public static CustomLeaderboardRankSorting ToDomain(this MainApi.CustomLeaderboardRankSorting rankSorting) => rankSorting switch
	{
		MainApi.CustomLeaderboardRankSorting.TimeDesc => CustomLeaderboardRankSorting.TimeDesc,
		MainApi.CustomLeaderboardRankSorting.TimeAsc => CustomLeaderboardRankSorting.TimeAsc,
		MainApi.CustomLeaderboardRankSorting.GemsDesc => CustomLeaderboardRankSorting.GemsDesc,
		MainApi.CustomLeaderboardRankSorting.KillsDesc => CustomLeaderboardRankSorting.KillsDesc,
		MainApi.CustomLeaderboardRankSorting.HomingDesc => CustomLeaderboardRankSorting.HomingDesc,
		_ => throw new UnreachableException(),
	};
}
