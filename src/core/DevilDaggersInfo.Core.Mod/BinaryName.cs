using DevilDaggersInfo.Types.Core;

namespace DevilDaggersInfo.Core.Mod;

public readonly record struct BinaryName(ModBinaryType BinaryType, string Name)
{
	public string ToFullName(string modName)
		=> $"{BinaryType.ToString().ToLower()}-{modName}-{Name}";

	public static BinaryName Parse(string fullName, string modName)
	{
		string modNameWithTrivia = $"-{modName}-";

		if (!fullName.Contains(modNameWithTrivia))
			throw new ArgumentException($"The string {fullName} does not contain -{modName}-. The binary name cannot be determined.", nameof(modName));

		string[] parsedValues = fullName.Split(modNameWithTrivia);
		if (parsedValues.Length != 2)
			throw new ArgumentException($"The string {fullName} is invalid. The binary name cannot be determined.", nameof(modName));

		ModBinaryType modBinaryType = parsedValues[0] switch
		{
			"dd" => ModBinaryType.Dd,
			"audio" => ModBinaryType.Audio,
			_ => throw new ArgumentException($"The string {fullName} contains an invalid binary type prefix.", nameof(modName)),
		};

		return new(modBinaryType, parsedValues[1]);
	}
}
