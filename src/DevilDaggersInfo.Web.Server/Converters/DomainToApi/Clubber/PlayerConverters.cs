using DevilDaggersInfo.Web.ApiSpec.Clubber.Players;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Clubber;

public static class PlayerConverters
{
	public static GetPlayerHistory ToClubberApi(this PlayerHistory playerHistory) => new()
	{
		Usernames = playerHistory.Usernames,
		ActivityHistory = playerHistory.ActivityHistory.ConvertAll(ah => new GetPlayerHistoryActivityEntry
		{
			DateTime = ah.DateTime,
			DeathsIncrement = ah.DeathsIncrement,
			TimeIncrement = ah.TimeIncrement,
		}),
		BestRank = playerHistory.BestRank,
		RankHistory = playerHistory.RankHistory.ConvertAll(rh => new GetPlayerHistoryRankEntry
		{
			DateTime = rh.DateTime,
			Rank = rh.Rank,
		}),
		ScoreHistory = playerHistory.ScoreHistory.ConvertAll(sh => new GetPlayerHistoryScoreEntry
		{
			DateTime = sh.DateTime,
			Gems = sh.Gems,
			Kills = sh.Kills,
			Rank = sh.Rank,
			Time = sh.Time,
			Username = sh.Username,
			DaggersFired = sh.DaggersFired,
			DaggersHit = sh.DaggersHit,
			DeathType = sh.DeathType,
		}),
		HidePastUsernames = playerHistory.HidePastUsernames,
	};
}
