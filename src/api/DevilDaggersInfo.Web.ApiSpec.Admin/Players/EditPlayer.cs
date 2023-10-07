using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Api.Admin.Players;

public record EditPlayer
{
	[StringLength(32)]
	public required string? CommonName { get; init; }

	// UInt64 not supported by Blazor InputNumber
	public required long? DiscordUserId { get; init; }

	[StringLength(2)]
	public required string? CountryCode { get; init; }

	[Range(1, 20000)]
	public required int? Dpi { get; init; }

	[Range(0.01f, 2)]
	public required float? InGameSens { get; init; }

	[Range(0, 179)]
	public required int? Fov { get; init; }

	public required bool? IsRightHanded { get; init; }

	public required bool? HasFlashHandEnabled { get; init; }

	[Range(0.5f, 5)]
	public required float? Gamma { get; init; }

	public required bool? UsesLegacyAudio { get; init; }

	public required bool? UsesHrtf { get; init; }

	public required bool? UsesInvertY { get; init; }

	public required VerticalSync VerticalSync { get; init; }

	public required BanType BanType { get; init; }

	[StringLength(64)]
	public required string? BanDescription { get; init; }

	[Range(1, int.MaxValue)]
	public required int? BanResponsibleId { get; init; }

	public required bool IsBannedFromDdcl { get; init; }

	public required bool HideSettings { get; init; }

	public required bool HideDonations { get; init; }

	public required bool HidePastUsernames { get; init; }

	public required List<int>? ModIds { get; init; }
}
