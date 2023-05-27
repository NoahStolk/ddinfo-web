using DevilDaggersInfo.Common;
using ApiAdmin = DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using ApiMain = DevilDaggersInfo.Api.Main.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Client.Extensions;

public static class CustomLeaderboardRankSortingExtensions
{
	public static string GetDaggerValue(this ApiMain.CustomLeaderboardRankSorting apiRankSorting, double apiValue)
	{
		// TODO: Use IsTime extension method.
		bool isTime = apiRankSorting is ApiMain.CustomLeaderboardRankSorting.TimeDesc or ApiMain.CustomLeaderboardRankSorting.TimeAsc;
		return apiValue.ToString(isTime ? StringFormats.TimeFormat : "0");
	}

	public static string GetDaggerValue(this ApiAdmin.CustomLeaderboardRankSorting apiRankSorting, double apiValue)
	{
		// TODO: Use IsTime extension method.
		bool isTime = apiRankSorting is ApiAdmin.CustomLeaderboardRankSorting.TimeDesc or ApiAdmin.CustomLeaderboardRankSorting.TimeAsc;
		return apiValue.ToString(isTime ? StringFormats.TimeFormat : "0");
	}
}
