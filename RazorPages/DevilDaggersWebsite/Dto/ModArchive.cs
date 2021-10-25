using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto
{
	public class ModArchive
	{
		public ModArchive(long fileSize, long fileSizeExtracted, List<ModBinary> binaries)
		{
			FileSize = fileSize;
			FileSizeExtracted = fileSizeExtracted;
			Binaries = binaries;
		}

		public long FileSize { get; }
		public long FileSizeExtracted { get; }
		public List<ModBinary> Binaries { get; }
	}
}
