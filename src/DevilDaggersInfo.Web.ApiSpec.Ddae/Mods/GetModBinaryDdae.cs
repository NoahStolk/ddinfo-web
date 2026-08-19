namespace DevilDaggersInfo.Web.ApiSpec.Ddae.Mods;

public sealed record GetModBinaryDdae
{
	public required string Name { get; init; }

	public long Size { get; init; }

	public ModBinaryTypeDdae ModBinaryType { get; init; }
}
