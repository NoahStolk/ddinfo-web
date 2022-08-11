using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Commands.Players;

public record EditPlayer
{
	public int Id { get; init; }

	public string? CommonName { get; init; }

	public long? DiscordUserId { get; init; }

	public string? CountryCode { get; init; }

	public int? Dpi { get; init; }

	public float? InGameSens { get; init; }

	public int? Fov { get; init; }

	public bool? IsRightHanded { get; init; }

	public bool? HasFlashHandEnabled { get; init; }

	public float? Gamma { get; init; }

	public bool? UsesLegacyAudio { get; init; }

	public bool? UsesHrtf { get; init; }

	public bool? UsesInvertY { get; init; }

	public VerticalSync VerticalSync { get; init; }

	public BanType BanType { get; init; }

	public string? BanDescription { get; init; }

	public int? BanResponsibleId { get; init; }

	public bool IsBannedFromDdcl { get; init; }

	public bool HideSettings { get; init; }

	public bool HideDonations { get; init; }

	public bool HidePastUsernames { get; init; }

	public List<int>? ModIds { get; init; }
}
