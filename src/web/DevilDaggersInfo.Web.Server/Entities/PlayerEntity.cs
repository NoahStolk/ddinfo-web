namespace DevilDaggersInfo.Web.Server.Entities;

[Table("Players")]
public class PlayerEntity
{
	[Key]
	public int Id { get; set; }

	[StringLength(32)]
	public string PlayerName { get; set; } = null!;

	[StringLength(32)]
	public string? CommonName { get; set; }

	public ulong? DiscordUserId { get; set; }

	[StringLength(2)]
	public string? CountryCode { get; set; }

	public int? Dpi { get; set; }

	public float? InGameSens { get; set; }

	public int? Fov { get; set; }

	public bool? IsRightHanded { get; set; }

	public bool? HasFlashHandEnabled { get; set; }

	public float? Gamma { get; set; }

	public bool? UsesLegacyAudio { get; set; }

	public bool? UsesHrtf { get; set; }

	public bool? UsesInvertY { get; set; }

	public VerticalSync VerticalSync { get; set; }

	public BanType BanType { get; set; }

	[StringLength(64)]
	public string? BanDescription { get; set; }

	public int? BanResponsibleId { get; set; }

	public bool IsBannedFromDdcl { get; set; }

	public bool HideSettings { get; set; }

	public bool HideDonations { get; set; }

	public bool HidePastUsernames { get; set; }

	public List<PlayerModEntity> PlayerMods { get; set; } = new();
}
