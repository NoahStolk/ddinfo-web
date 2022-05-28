namespace DevilDaggersInfo.Web.Shared.Dto.DdstatsRust.Access;

public record DdstatsRustAccessInfo
{
	public Version RequiredVersion { get; init; } = null!;
}
