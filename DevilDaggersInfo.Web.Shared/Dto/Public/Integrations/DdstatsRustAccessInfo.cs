namespace DevilDaggersInfo.Web.Shared.Dto.Public.Integrations;

public record DdstatsRustAccessInfo
{
	public Version RequiredVersion { get; init; } = null!;
}
