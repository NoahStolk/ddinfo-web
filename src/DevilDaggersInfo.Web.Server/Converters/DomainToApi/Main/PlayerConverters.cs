using DevilDaggersInfo.Web.ApiSpec.Main.Players;
using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;

public static class PlayerConverters
{
	public static GetPlayerForLeaderboard ToMainApi(this PlayerForLeaderboard player)
	{
		return new()
		{
			BanDescription = player.BanDescription,
			BanResponsibleId = player.BanResponsibleId,
			BanType = player.BanType.ToMainApi(),
			CountryCode = player.CountryCode,
			Id = player.Id,
		};
	}

	public static GetPlayerForSettings ToMainApi(this PlayerForSettings player)
	{
		return new()
		{
			CountryCode = player.CountryCode,
			Id = player.Id,
			Settings = player.Settings.ToMainApi(),
		};
	}

	public static GetPlayer ToMainApi(this Player player)
	{
		return new()
		{
			BanDescription = player.BanDescription,
			CountryCode = player.CountryCode,
			Id = player.Id,
			IsBanned = player.IsBanned,
			IsPublicDonor = player.IsPublicDonor,
			Settings = player.Settings?.ToMainApi(),
		};
	}

	private static GetPlayerSettings ToMainApi(this PlayerSettings settings)
	{
		return new()
		{
			Dpi = settings.Dpi,
			Fov = settings.Fov,
			Gamma = settings.Gamma,
			InGameSens = settings.InGameSens,
			IsRightHanded = settings.IsRightHanded,
			UsesFlashHand = settings.UsesFlashHand,
			UsesLegacyAudio = settings.UsesLegacyAudio,
			UsesHrtf = settings.UsesHrtf,
			UsesInvertY = settings.UsesInvertY,
			VerticalSync = settings.VerticalSync.ToMainApi(),
		};
	}

	private static BanType ToMainApi(this Domain.Entities.Enums.BanType banType)
	{
		return banType switch
		{
			Domain.Entities.Enums.BanType.NotBanned => BanType.NotBanned,
			Domain.Entities.Enums.BanType.Alt => BanType.Alt,
			Domain.Entities.Enums.BanType.Cheater => BanType.Cheater,
			Domain.Entities.Enums.BanType.Boosted => BanType.Boosted,
			Domain.Entities.Enums.BanType.IllegitimateStats => BanType.IllegitimateStats,
			Domain.Entities.Enums.BanType.BlankName => BanType.BlankName,
			_ => throw new UnreachableException(),
		};
	}

	private static VerticalSync ToMainApi(this Domain.Entities.Enums.VerticalSync verticalSync)
	{
		return verticalSync switch
		{
			Domain.Entities.Enums.VerticalSync.Unknown => VerticalSync.Unknown,
			Domain.Entities.Enums.VerticalSync.Off => VerticalSync.Off,
			Domain.Entities.Enums.VerticalSync.On => VerticalSync.On,
			Domain.Entities.Enums.VerticalSync.Adaptive => VerticalSync.Adaptive,
			_ => throw new UnreachableException(),
		};
	}

	public static GetPlayerHistory ToMainApi(this PlayerHistory playerHistory)
	{
		return new()
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
}
