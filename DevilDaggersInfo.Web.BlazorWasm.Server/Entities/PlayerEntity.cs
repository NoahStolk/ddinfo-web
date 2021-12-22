namespace DevilDaggersInfo.Web.BlazorWasm.Server.Entities;

[Table("Players")]
public class PlayerEntity
{
	[Key]
	public int Id { get; set; }

	[StringLength(32)]
	public string PlayerName { get; set; } = null!;

	[StringLength(2)]
	public string? CountryCode { get; set; }

	public int? Dpi { get; set; }

	public float? InGameSens { get; set; }

	public int? Fov { get; set; }

	public bool? IsRightHanded { get; set; }

	public bool? HasFlashHandEnabled { get; set; }

	public float? Gamma { get; set; }

	public bool? UsesLegacyAudio { get; set; }

	// TODO: Remove and use BanType instead.
	[Obsolete($"Use {nameof(BanType)} instead.")]
	public bool IsBanned { get; set; }

	public BanType BanType { get; set; }

	[StringLength(64)]
	public string? BanDescription { get; set; }

	public int? BanResponsibleId { get; set; }

	public bool IsBannedFromDdcl { get; set; }

	public bool HideSettings { get; set; }

	public bool HideDonations { get; set; }

	public bool HidePastUsernames { get; set; }

	public List<PlayerModEntity> PlayerMods { get; set; } = new();

	public List<PlayerTitleEntity> PlayerTitles { get; set; } = new();
}
