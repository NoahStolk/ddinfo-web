using DevilDaggersInfo.Types.Core.Mods;

namespace DevilDaggersInfo.Api.Ddae.Mods;

public record GetModBinaryDdae
{
	public required string Name { get; init; }

	public long Size { get; init; }

	public ModBinaryType ModBinaryType { get; init; }
}
