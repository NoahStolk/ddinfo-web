using DevilDaggersInfo.Web.ApiSpec.Admin.Players;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using System.Diagnostics;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Converters.DomainToApi;

public static class PlayerConverters
{
	public static GetPlayerForOverview ToAdminApiOverview(this PlayerEntity player)
	{
		return new GetPlayerForOverview
		{
			Id = player.Id,
			PlayerName = player.PlayerName,
			CommonName = player.CommonName,
			DiscordUserId = player.DiscordUserId,
			CountryCode = player.CountryCode,
			Dpi = player.Dpi,
			InGameSens = player.InGameSens,
			Fov = player.Fov,
			BanType = player.BanType.ToAdminApi(),
			BanDescription = player.BanDescription,
			BanResponsibleId = player.BanResponsibleId,
			Gamma = player.Gamma,
			HasFlashHandEnabled = player.HasFlashHandEnabled,
			HideDonations = player.HideDonations,
			HidePastUsernames = player.HidePastUsernames,
			HideSettings = player.HideSettings,
			IsBannedFromDdcl = player.IsBannedFromDdcl,
			IsRightHanded = player.IsRightHanded,
			UsesLegacyAudio = player.UsesLegacyAudio,
			UsesHrtf = player.UsesHrtf,
			UsesInvertY = player.UsesInvertY,
			VerticalSync = player.VerticalSync.ToAdminApi(),
		};
	}

	public static GetPlayer ToAdminApi(this PlayerEntity player)
	{
		if (player.PlayerMods == null)
			throw new InvalidOperationException("Player mods are not included.");

		return new GetPlayer
		{
			Id = player.Id,
			CommonName = player.CommonName,
			DiscordUserId = player.DiscordUserId,
			CountryCode = player.CountryCode,
			Dpi = player.Dpi,
			InGameSens = player.InGameSens,
			Fov = player.Fov,
			IsRightHanded = player.IsRightHanded,
			HasFlashHandEnabled = player.HasFlashHandEnabled,
			Gamma = player.Gamma,
			UsesLegacyAudio = player.UsesLegacyAudio,
			UsesHrtf = player.UsesHrtf,
			UsesInvertY = player.UsesInvertY,
			VerticalSync = player.VerticalSync.ToAdminApi(),
			BanType = player.BanType.ToAdminApi(),
			BanDescription = player.BanDescription,
			BanResponsibleId = player.BanResponsibleId,
			IsBannedFromDdcl = player.IsBannedFromDdcl,
			HideSettings = player.HideSettings,
			HideDonations = player.HideDonations,
			HidePastUsernames = player.HidePastUsernames,
			ModIds = player.PlayerMods.ConvertAll(pam => pam.ModId),
		};
	}

	private static BanType ToAdminApi(this Entities.Enums.BanType banType)
	{
		return banType switch
		{
			Entities.Enums.BanType.NotBanned => BanType.NotBanned,
			Entities.Enums.BanType.Alt => BanType.Alt,
			Entities.Enums.BanType.Cheater => BanType.Cheater,
			Entities.Enums.BanType.Boosted => BanType.Boosted,
			Entities.Enums.BanType.IllegitimateStats => BanType.IllegitimateStats,
			Entities.Enums.BanType.BlankName => BanType.BlankName,
			_ => throw new UnreachableException(),
		};
	}

	private static VerticalSync ToAdminApi(this Entities.Enums.VerticalSync verticalSync)
	{
		return verticalSync switch
		{
			Entities.Enums.VerticalSync.Unknown => VerticalSync.Unknown,
			Entities.Enums.VerticalSync.Off => VerticalSync.Off,
			Entities.Enums.VerticalSync.On => VerticalSync.On,
			Entities.Enums.VerticalSync.Adaptive => VerticalSync.Adaptive,
			_ => throw new UnreachableException(),
		};
	}
}
