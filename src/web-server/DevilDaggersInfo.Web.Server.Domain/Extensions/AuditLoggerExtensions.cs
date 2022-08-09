using DevilDaggersInfo.Web.Server.Domain.Commands.Players;
using DevilDaggersInfo.Web.Server.Domain.Commands.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

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

	public static Dictionary<string, string> GetLog(this EditPlayerProfile player)
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
		AddProperty(log, player.UsesHrtf);
		AddProperty(log, player.UsesInvertY);
		AddProperty(log, player.VerticalSync);
		AddProperty(log, player.HideSettings);
		AddProperty(log, player.HideDonations);
		AddProperty(log, player.HidePastUsernames);
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
