namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Integrations;

public record DdstatsRustAccessInfo
{
	public Version RequiredVersion { get; init; } = null!;
}
