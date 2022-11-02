using DevilDaggersInfo.Types.Web;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.Players;

public record EditPlayer
{
	[StringLength(32)]
	public string? CommonName { get; init; }

	// UInt64 not supported by Blazor InputNumber
	public long? DiscordUserId { get; init; }

	[StringLength(2)]
	public string? CountryCode { get; init; }

	[Range(1, 20000)]
	public int? Dpi { get; init; }

	[Range(0.01f, 2)]
	public float? InGameSens { get; init; }

	[Range(0, 179)]
	public int? Fov { get; init; }

	public bool? IsRightHanded { get; init; }

	public bool? HasFlashHandEnabled { get; init; }

	[Range(0.5f, 5)]
	public float? Gamma { get; init; }

	public bool? UsesLegacyAudio { get; init; }

	public bool? UsesHrtf { get; init; }

	public bool? UsesInvertY { get; init; }

	public VerticalSync VerticalSync { get; init; }

	public BanType BanType { get; init; }

	[StringLength(64)]
	public string? BanDescription { get; init; }

	[Range(1, int.MaxValue)]
	public int? BanResponsibleId { get; init; }

	public bool IsBannedFromDdcl { get; init; }

	public bool HideSettings { get; init; }

	public bool HideDonations { get; init; }

	public bool HidePastUsernames { get; init; }

	public List<int>? ModIds { get; init; }
}
