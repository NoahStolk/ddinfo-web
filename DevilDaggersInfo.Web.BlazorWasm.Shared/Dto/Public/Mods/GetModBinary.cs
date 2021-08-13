using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods
{
	public class GetModBinary
	{
		public string Name { get; init; } = null!;
		public long Size { get; init; }
		public ModBinaryType ModBinaryType { get; init; }
	}
}
