using DevilDaggersWebsite.BlazorWasm.Shared.Enums;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Mods
{
	public class ModBinaryPublic
	{
		public string Name { get; init; } = null!;
		public long Size { get; init; }
		public ModBinaryType ModBinaryType { get; init; }
	}
}
