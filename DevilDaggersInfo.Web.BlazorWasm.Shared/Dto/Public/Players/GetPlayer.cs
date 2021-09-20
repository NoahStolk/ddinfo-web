namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;

public class GetPlayer : IGetDto<int>
{
	public int Id { get; init; }

	public bool IsBanned { get; init; }

	public string? BanDescription { get; init; }

	public List<string> Titles { get; init; } = null!;

	public bool IsPublicDonator { get; init; }

	public string? CountryCode { get; init; }

	public int? Dpi { get; init; }

	public float? InGameSens { get; init; }

	public int? Fov { get; init; }

	public bool? IsRightHanded { get; init; }

	public bool? HasFlashHandEnabled { get; init; }

	public float? Gamma { get; init; }

	public bool? UsesLegacyAudio { get; init; }

	public float? Edpi => Dpi * InGameSens;
}
