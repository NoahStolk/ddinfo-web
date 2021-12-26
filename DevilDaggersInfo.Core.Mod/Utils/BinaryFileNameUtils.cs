namespace DevilDaggersInfo.Core.Mod.Utils;

public static class BinaryFileNameUtils
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

	public static string RemoveBinaryPrefix(string fileName, string modName)
	{
		if (fileName.StartsWith("audio"))
			return fileName.Replace(GetBinaryPrefix(ModBinaryType.Audio, modName), string.Empty);

		if (fileName.StartsWith("core"))
			return fileName.Replace(GetBinaryPrefix(ModBinaryType.Core, modName), string.Empty);

		if (fileName.StartsWith("dd"))
			return fileName.Replace(GetBinaryPrefix(ModBinaryType.Dd, modName), string.Empty);

		return fileName;
	}

#pragma warning disable S3267 // Loops should be simplified with "LINQ" expressions
	public static string SanitizeModBinaryFileName(string fileName, string modName)
	{
		List<(ModBinaryType Type, string Prefix)> typePrefixesToRemove = new();
		foreach (ModBinaryType type in Enum.GetValues<ModBinaryType>())
		{
			string typeString = type.ToString().ToLower();

			// Check prefixes with separators (_ or -) first so the separators aren't kept.
			typePrefixesToRemove.Add((type, $"_{typeString}_"));
			typePrefixesToRemove.Add((type, $"_{typeString}-"));
			typePrefixesToRemove.Add((type, $"_{typeString}"));
			typePrefixesToRemove.Add((type, $"{typeString}_"));
			typePrefixesToRemove.Add((type, $"{typeString}-"));
			typePrefixesToRemove.Add((type, typeString));
		}

		ModBinaryType? modBinaryType = null;
		string binaryName = fileName;
		foreach ((ModBinaryType type, string prefix) in typePrefixesToRemove)
		{
			if (binaryName.StartsWith(prefix))
			{
				binaryName = binaryName.Replace(prefix, string.Empty);
				modBinaryType = type;
			}
		}

		foreach (string namePrefix in new[] { $"{modName}_", $"{modName}-", modName })
		{
			if (binaryName.StartsWith(namePrefix))
				binaryName = binaryName.Replace(namePrefix, string.Empty);
		}

		return $"{(modBinaryType ?? ModBinaryType.Dd).ToString().ToLower()}-{modName}-{binaryName}";
	}
#pragma warning restore S3267 // Loops should be simplified with "LINQ" expressions
}
