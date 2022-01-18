using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Donations;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Spawnsets;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Extensions;

public static class AuditLoggerExtensions
{
	public static Dictionary<string, string> GetLog(this AddSpawnset spawnset)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, spawnset.Name);
		AddProperty(log, spawnset.PlayerId);
		AddProperty(log, spawnset.MaxDisplayWaves);
		AddProperty(log, spawnset.IsPractice);
		AddProperty(log, spawnset.HtmlDescription);
		log.Add(nameof(AddSpawnset.FileContents), FileSizeUtils.Format(spawnset.FileContents.Length));
		return log;
	}

	public static Dictionary<string, string> GetLog(this EditSpawnset spawnset)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, spawnset.Name);
		AddProperty(log, spawnset.PlayerId);
		AddProperty(log, spawnset.MaxDisplayWaves);
		AddProperty(log, spawnset.IsPractice);
		AddProperty(log, spawnset.HtmlDescription);
		return log;
	}

	public static Dictionary<string, string> GetLog(this SpawnsetEntity spawnset)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, spawnset.Id);
		AddProperty(log, spawnset.Name);
		AddProperty(log, spawnset.PlayerId);
		AddProperty(log, spawnset.MaxDisplayWaves);
		AddProperty(log, spawnset.IsPractice);
		AddProperty(log, spawnset.HtmlDescription);
		AddProperty(log, spawnset.LastUpdated);
		return log;
	}

	public static Dictionary<string, string> GetLog(this AddMod mod)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, mod.Name);
		log.Add(nameof(mod.PlayerIds), mod.PlayerIds == null ? string.Empty : string.Join(", ", mod.PlayerIds.ConvertAll(i => i.ToString())));
		AddProperty(log, mod.IsHidden);
		AddProperty(log, mod.HtmlDescription);
		AddProperty(log, mod.ModTypes?.ToFlagEnum<ModTypes>());
		AddProperty(log, mod.TrailerUrl);
		AddProperty(log, mod.Url);
		return log;
	}

	public static Dictionary<string, string> GetLog(this EditMod mod)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, mod.Name);
		log.Add(nameof(mod.PlayerIds), mod.PlayerIds == null ? string.Empty : string.Join(", ", mod.PlayerIds.ConvertAll(i => i.ToString())));
		AddProperty(log, mod.IsHidden);
		AddProperty(log, mod.HtmlDescription);
		AddProperty(log, mod.ModTypes?.ToFlagEnum<ModTypes>());
		AddProperty(log, mod.TrailerUrl);
		AddProperty(log, mod.Url);
		return log;
	}

	public static Dictionary<string, string> GetLog(this ModEntity mod)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, mod.Id);
		AddProperty(log, mod.Name);
		AddProperty(log, mod.IsHidden);
		AddProperty(log, mod.HtmlDescription);
		AddProperty(log, mod.ModTypes);
		AddProperty(log, mod.TrailerUrl);
		AddProperty(log, mod.Url);
		AddProperty(log, mod.LastUpdated);
		return log;
	}

	public static Dictionary<string, string> GetLog(this AddCustomEntry customEntry)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, customEntry.CustomLeaderboardId);
		AddProperty(log, customEntry.PlayerId);
		AddProperty(log, customEntry.Time);
		AddProperty(log, customEntry.GemsCollected);
		AddProperty(log, customEntry.EnemiesKilled);
		AddProperty(log, customEntry.DaggersFired);
		AddProperty(log, customEntry.DaggersHit);
		AddProperty(log, customEntry.EnemiesAlive);
		AddProperty(log, customEntry.HomingStored);
		AddProperty(log, customEntry.HomingEaten);
		AddProperty(log, customEntry.GemsDespawned);
		AddProperty(log, customEntry.GemsEaten);
		AddProperty(log, customEntry.GemsTotal);
		AddProperty(log, customEntry.DeathType);
		AddProperty(log, customEntry.LevelUpTime2);
		AddProperty(log, customEntry.LevelUpTime3);
		AddProperty(log, customEntry.LevelUpTime4);
		AddProperty(log, customEntry.SubmitDate);
		AddProperty(log, customEntry.ClientVersion);
		return log;
	}

	public static Dictionary<string, string> GetLog(this EditCustomEntry customEntry)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, customEntry.CustomLeaderboardId);
		AddProperty(log, customEntry.PlayerId);
		AddProperty(log, customEntry.Time);
		AddProperty(log, customEntry.GemsCollected);
		AddProperty(log, customEntry.EnemiesKilled);
		AddProperty(log, customEntry.DaggersFired);
		AddProperty(log, customEntry.DaggersHit);
		AddProperty(log, customEntry.EnemiesAlive);
		AddProperty(log, customEntry.HomingStored);
		AddProperty(log, customEntry.HomingEaten);
		AddProperty(log, customEntry.GemsDespawned);
		AddProperty(log, customEntry.GemsEaten);
		AddProperty(log, customEntry.GemsTotal);
		AddProperty(log, customEntry.DeathType);
		AddProperty(log, customEntry.LevelUpTime2);
		AddProperty(log, customEntry.LevelUpTime3);
		AddProperty(log, customEntry.LevelUpTime4);
		AddProperty(log, customEntry.SubmitDate);
		AddProperty(log, customEntry.ClientVersion);
		return log;
	}

	public static Dictionary<string, string> GetLog(this CustomEntryEntity customEntry)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, customEntry.Id);
		AddProperty(log, customEntry.CustomLeaderboardId);
		AddProperty(log, customEntry.PlayerId);
		AddProperty(log, customEntry.Time);
		AddProperty(log, customEntry.GemsCollected);
		AddProperty(log, customEntry.EnemiesKilled);
		AddProperty(log, customEntry.DaggersFired);
		AddProperty(log, customEntry.DaggersHit);
		AddProperty(log, customEntry.EnemiesAlive);
		AddProperty(log, customEntry.HomingStored);
		AddProperty(log, customEntry.HomingEaten);
		AddProperty(log, customEntry.GemsDespawned);
		AddProperty(log, customEntry.GemsEaten);
		AddProperty(log, customEntry.GemsTotal);
		AddProperty(log, customEntry.DeathType);
		AddProperty(log, customEntry.LevelUpTime2);
		AddProperty(log, customEntry.LevelUpTime3);
		AddProperty(log, customEntry.LevelUpTime4);
		AddProperty(log, customEntry.SubmitDate);
		AddProperty(log, customEntry.ClientVersion);
		AddProperty(log, customEntry.Client);
		return log;
	}

	public static Dictionary<string, string> GetLog(this AddCustomLeaderboard customLeaderboard)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, customLeaderboard.SpawnsetId);
		AddProperty(log, customLeaderboard.Category);
		AddProperty(log, customLeaderboard.TimeBronze);
		AddProperty(log, customLeaderboard.TimeSilver);
		AddProperty(log, customLeaderboard.TimeGolden);
		AddProperty(log, customLeaderboard.TimeDevil);
		AddProperty(log, customLeaderboard.TimeLeviathan);
		return log;
	}

	public static Dictionary<string, string> GetLog(this EditCustomLeaderboard customLeaderboard)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, customLeaderboard.Category);
		AddProperty(log, customLeaderboard.TimeBronze);
		AddProperty(log, customLeaderboard.TimeSilver);
		AddProperty(log, customLeaderboard.TimeGolden);
		AddProperty(log, customLeaderboard.TimeDevil);
		AddProperty(log, customLeaderboard.TimeLeviathan);
		AddProperty(log, customLeaderboard.IsArchived);
		return log;
	}

	public static Dictionary<string, string> GetLog(this CustomLeaderboardEntity customLeaderboard)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, customLeaderboard.Id);
		AddProperty(log, customLeaderboard.SpawnsetId);
		AddProperty(log, customLeaderboard.Category);
		AddProperty(log, customLeaderboard.TimeBronze);
		AddProperty(log, customLeaderboard.TimeSilver);
		AddProperty(log, customLeaderboard.TimeGolden);
		AddProperty(log, customLeaderboard.TimeDevil);
		AddProperty(log, customLeaderboard.TimeLeviathan);
		AddProperty(log, customLeaderboard.DateCreated);
		AddProperty(log, customLeaderboard.DateLastPlayed);
		AddProperty(log, customLeaderboard.IsArchived);
		AddProperty(log, customLeaderboard.TotalRunsSubmitted);
		return log;
	}

	public static Dictionary<string, string> GetLog(this AddPlayer player)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, player.Id);
		AddProperty(log, player.CountryCode);
		AddProperty(log, player.Dpi);
		AddProperty(log, player.InGameSens);
		AddProperty(log, player.Fov);
		AddProperty(log, player.IsRightHanded);
		AddProperty(log, player.HasFlashHandEnabled);
		AddProperty(log, player.Gamma);
		AddProperty(log, player.UsesLegacyAudio);
		AddProperty(log, player.BanType);
		AddProperty(log, player.BanDescription);
		AddProperty(log, player.BanResponsibleId);
		AddProperty(log, player.IsBannedFromDdcl);
		AddProperty(log, player.HideSettings);
		AddProperty(log, player.HideDonations);
		AddProperty(log, player.HidePastUsernames);
		log.Add(nameof(player.ModIds), player.ModIds == null ? string.Empty : string.Join(", ", player.ModIds.ConvertAll(i => i.ToString())));
		return log;
	}

	public static Dictionary<string, string> GetLog(this EditPlayer player)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, player.CountryCode);
		AddProperty(log, player.Dpi);
		AddProperty(log, player.InGameSens);
		AddProperty(log, player.Fov);
		AddProperty(log, player.IsRightHanded);
		AddProperty(log, player.HasFlashHandEnabled);
		AddProperty(log, player.Gamma);
		AddProperty(log, player.UsesLegacyAudio);
		AddProperty(log, player.BanType);
		AddProperty(log, player.BanDescription);
		AddProperty(log, player.BanResponsibleId);
		AddProperty(log, player.IsBannedFromDdcl);
		AddProperty(log, player.HideSettings);
		AddProperty(log, player.HideDonations);
		AddProperty(log, player.HidePastUsernames);
		log.Add(nameof(player.ModIds), player.ModIds == null ? string.Empty : string.Join(", ", player.ModIds.ConvertAll(i => i.ToString())));
		return log;
	}

	public static Dictionary<string, string> GetLog(this PlayerEntity player)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, player.Id);
		AddProperty(log, player.PlayerName);
		AddProperty(log, player.CountryCode);
		AddProperty(log, player.Dpi);
		AddProperty(log, player.InGameSens);
		AddProperty(log, player.Fov);
		AddProperty(log, player.IsRightHanded);
		AddProperty(log, player.HasFlashHandEnabled);
		AddProperty(log, player.Gamma);
		AddProperty(log, player.UsesLegacyAudio);
		AddProperty(log, player.BanType);
		AddProperty(log, player.BanDescription);
		AddProperty(log, player.BanResponsibleId);
		AddProperty(log, player.IsBannedFromDdcl);
		AddProperty(log, player.HideSettings);
		AddProperty(log, player.HideDonations);
		AddProperty(log, player.HidePastUsernames);
		return log;
	}

	public static Dictionary<string, string> GetLog(this AddDonation donation)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, donation.PlayerId);
		AddProperty(log, donation.Amount);
		AddProperty(log, donation.ConvertedEuroCentsReceived);
		AddProperty(log, donation.Currency);
		AddProperty(log, donation.DateReceived);
		AddProperty(log, donation.IsRefunded);
		AddProperty(log, donation.Note);
		return log;
	}

	public static Dictionary<string, string> GetLog(this EditDonation donation)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, donation.PlayerId);
		AddProperty(log, donation.Amount);
		AddProperty(log, donation.ConvertedEuroCentsReceived);
		AddProperty(log, donation.Currency);
		AddProperty(log, donation.DateReceived);
		AddProperty(log, donation.IsRefunded);
		AddProperty(log, donation.Note);
		return log;
	}

	public static Dictionary<string, string> GetLog(this DonationEntity donation)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, donation.Id);
		AddProperty(log, donation.PlayerId);
		AddProperty(log, donation.Amount);
		AddProperty(log, donation.ConvertedEuroCentsReceived);
		AddProperty(log, donation.Currency);
		AddProperty(log, donation.DateReceived);
		AddProperty(log, donation.IsRefunded);
		AddProperty(log, donation.Note);
		return log;
	}

	public static Dictionary<string, string> GetLog(this UserEntity user)
	{
		Dictionary<string, string> log = new();
		AddProperty(log, user.Id);
		AddProperty(log, user.Name);
		return log;
	}

	private static void AddProperty(Dictionary<string, string> dictionary, object? value, [CallerArgumentExpression("value")] string? valueExpression = null)
	{
		valueExpression ??= "NULL";

		int pos = valueExpression.IndexOf('.');
		if (pos != -1)
			valueExpression = valueExpression[(pos + 1)..];

		if (!dictionary.ContainsKey(valueExpression))
			dictionary.Add(valueExpression, value?.ToString() ?? string.Empty);
	}
}
