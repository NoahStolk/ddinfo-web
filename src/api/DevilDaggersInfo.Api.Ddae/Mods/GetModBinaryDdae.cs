using DevilDaggersInfo.Types.Core.Mods;

namespace DevilDaggersInfo.Api.Ddae.Mods;

public record GetModBinaryDdae
{
	public string Name { get; init; } = null!;

	public long Size { get; init; }

	public ModBinaryType ModBinaryType { get; init; }
}
