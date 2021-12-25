namespace DevilDaggersInfo.Core.Mod.Utils;

public static class ModFileNameUtils
{
	public static ModBinaryType GetBinaryTypeBasedOnFileName(string fileName)
	{
		if (fileName.StartsWith("audio"))
			return ModBinaryType.Audio;

		if (fileName.StartsWith("core"))
			return ModBinaryType.Core;

		if (fileName.StartsWith("dd"))
			return ModBinaryType.Dd;

		throw new InvalidModBinaryException($"Binary '{fileName}' must start with 'audio', 'core', or 'dd'.");
	}

	public static string GetBinaryPrefix(ModBinaryType modBinaryType, string modName)
	{
		return $"{modBinaryType.ToString().ToLower()}-{modName}-";
	}
}
