namespace DevilDaggersInfo.Api.Ddcl.Tools;

[Obsolete("DDCL 1.8.3 will be removed.")]
public class GetUpdate
{
	public required string VersionNumber { get; init; }

	public required string VersionNumberRequired { get; init; }
}
