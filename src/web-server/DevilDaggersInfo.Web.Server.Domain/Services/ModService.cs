using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class ModService
{
	public void ValidateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new AdminDomainException("Mod name must not be empty or consist of white space only.");

		if (name.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
			throw new AdminDomainException("Mod name must not contain invalid file name characters.");

		// Temporarily disallow + because it breaks old API calls where the mod name is in the URL. TODO: Remove this after old API calls have been removed.
		if (name.Any(c => c == '+'))
			throw new AdminDomainException("Mod name must not contain the + character.");
	}

	public Dictionary<BinaryName, byte[]> GetBinaryNames(List<(string Name, byte[] Data)> binaries)
	{
		Dictionary<BinaryName, byte[]> dict = new();

		foreach ((string name, byte[] data) in binaries)
		{
			ModBinary modBinary = new(data, ModBinaryReadComprehensiveness.TypeOnly);
			BinaryName binaryName = new(modBinary.ModBinaryType, name);
			if (dict.ContainsKey(binaryName))
				throw new InvalidModArchiveException("Binary names must all be unique.");

			dict.Add(binaryName, data);
		}

		return dict;
	}
}
