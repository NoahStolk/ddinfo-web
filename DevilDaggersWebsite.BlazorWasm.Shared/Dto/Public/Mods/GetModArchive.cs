using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Public.Mods
{
	public class GetModArchive
	{
		public long FileSize { get; init; }
		public long FileSizeExtracted { get; init; }
		public List<GetModBinary> Binaries { get; init; } = null!;
	}
}
