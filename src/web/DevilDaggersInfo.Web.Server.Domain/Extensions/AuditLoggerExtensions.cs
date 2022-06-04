using DevilDaggersInfo.Web.Server.Domain.Models.Players;
using System.Runtime.CompilerServices;

namespace DevilDaggersInfo.Web.Server.Domain.Extensions;

public static class AuditLoggerExtensions
{
	public static Dictionary<string, string> GetLog(this PlayerProfile player)
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
