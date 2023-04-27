namespace DevilDaggersInfo.Api.Admin.Players;

public record GetPlayerForOverview : IAdminOverviewGetDto
{
	public required int Id { get; init; }

	public required string PlayerName { get; init; }

	public required string? CommonName { get; init; }

	public required ulong? DiscordUserId { get; init; }

	public required string? CountryCode { get; init; }

	public required int? Dpi { get; init; }

	public required float? InGameSens { get; init; }

	public required int? Fov { get; init; }

	public required bool? IsRightHanded { get; init; }

	public required bool? HasFlashHandEnabled { get; init; }

	public required float? Gamma { get; init; }

	public required bool? UsesLegacyAudio { get; init; }

	public required bool? UsesHrtf { get; init; }

	public required bool? UsesInvertY { get; init; }

	public required VerticalSync VerticalSync { get; init; }

	public required BanType BanType { get; init; }

	public required string? BanDescription { get; init; }

	public required int? BanResponsibleId { get; init; }

	public required bool IsBannedFromDdcl { get; init; }

	public required bool HideSettings { get; init; }

	public required bool HideDonations { get; init; }

	public required bool HidePastUsernames { get; init; }
}
