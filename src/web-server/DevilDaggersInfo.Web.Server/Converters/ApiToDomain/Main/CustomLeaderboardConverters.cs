using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
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
		_ => throw new InvalidEnumConversionException(customLeaderboardSorting),
	};
}
