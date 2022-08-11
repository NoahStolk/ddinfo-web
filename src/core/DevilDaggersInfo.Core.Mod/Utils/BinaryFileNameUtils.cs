using DevilDaggersInfo.Types.Core.Mods;

namespace DevilDaggersInfo.Core.Mod.Utils;

public static class BinaryFileNameUtils
{
	public static ModBinaryType GetBinaryTypeBasedOnFileName(string fileName)
	{
		if (fileName.StartsWith("audio"))
			return ModBinaryType.Audio;

		if (fileName.StartsWith("dd"))
			return ModBinaryType.Dd;

		throw new InvalidModBinaryException($"Binary '{fileName}' must start with 'audio' or 'dd'.");
	}
}
