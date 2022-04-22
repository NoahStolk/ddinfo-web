namespace DevilDaggersInfo.Web.Shared.Dto.Admin.Players;

public record AddPlayer
{
	[Range(1, int.MaxValue)]
	public int Id { get; set; }

	[StringLength(32)]
	public string? CommonName { get; set; }

	// UInt64 not supported by Blazor InputNumber
	public long? DiscordUserId { get; set; }

	[StringLength(2)]
	public string? CountryCode { get; set; }

	[Range(1, 20000)]
	public int? Dpi { get; set; }

	[Range(0.01f, 2)]
	public float? InGameSens { get; set; }

	[Range(0, 179)]
	public int? Fov { get; set; }

	public bool? IsRightHanded { get; set; }

	public bool? HasFlashHandEnabled { get; set; }

	[Range(0.5f, 5)]
	public float? Gamma { get; set; }

	public bool? UsesLegacyAudio { get; set; }

	public bool? UsesHrtf { get; set; }

	public bool? UsesInvertY { get; set; }

	public VerticalSync VerticalSync { get; set; }

	public BanType BanType { get; set; }

	[StringLength(64)]
	public string? BanDescription { get; set; }

	[Range(1, int.MaxValue)]
	public int? BanResponsibleId { get; set; }

	public bool IsBannedFromDdcl { get; set; }

	public bool HideSettings { get; set; }

	public bool HideDonations { get; set; }

	public bool HidePastUsernames { get; set; }

	public List<int>? ModIds { get; set; }
}
