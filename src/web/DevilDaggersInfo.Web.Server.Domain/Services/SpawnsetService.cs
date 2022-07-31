using DevilDaggersInfo.Web.Server.Domain.Exceptions;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class SpawnsetService
{
	public void ValidateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new InvalidEntityNameException("Spawnset name must not be empty or consist of white space only.");

		if (name.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
			throw new InvalidEntityNameException("Spawnset name must not contain invalid file name characters.");

		// Temporarily disallow + because it breaks old API calls where the spawnset name is in the URL. TODO: Remove this after old API calls have been removed.
		if (name.Any(c => c == '+'))
			throw new InvalidEntityNameException("Spawnset name must not contain the + character.");
	}
}
