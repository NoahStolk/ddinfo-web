using System.Collections.Generic;

namespace DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods
{
	public class GetModArchive
	{
		public long FileSize { get; init; }
		public long FileSizeExtracted { get; init; }
		public List<GetModBinary> Binaries { get; init; } = null!;
	}
}
