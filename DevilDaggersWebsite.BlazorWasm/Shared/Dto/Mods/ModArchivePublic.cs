using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.Mods
{
	public class ModArchivePublic
	{
		public long FileSize { get; init; }
		public long FileSizeExtracted { get; init; }
		public List<ModBinaryPublic> Binaries { get; init; } = null!;
	}
}
