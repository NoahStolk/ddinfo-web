using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public sealed class ModScreenshotProcessor(IFileSystem fileSystem)
{
	public async Task ProcessModScreenshotUploadAsync(string modName, Dictionary<string, byte[]> screenshots)
	{
		if (screenshots.Count == 0)
			return;

		string prefix = ModArchiveAccessor.GetModScreenshotsPrefix(modName);
		HashSet<string> existingNames = [.. (await fileSystem.ListAsync(DataSubDirectory.ModScreenshots, prefix)).Select(f => f.Name)];
		int i = 0;
		foreach (byte[] screenshotContents in screenshots.OrderBy(kvp => kvp.Key).Select(kvp => kvp.Value).Where(PngFileUtils.HasValidPngHeader))
		{
			string name;
			do
			{
				name = $"{prefix}{i:00}.png";
				i++;
			}
			while (existingNames.Contains(name));

			await fileSystem.WriteAllBytesAsync(DataSubDirectory.ModScreenshots, name, screenshotContents);
		}
	}

	public async Task DeleteScreenshotAsync(string modName, string screenshotFileName)
	{
		await fileSystem.DeleteAsync(DataSubDirectory.ModScreenshots, $"{ModArchiveAccessor.GetModScreenshotsPrefix(modName)}{screenshotFileName}");
	}

	public async Task DeleteScreenshotsAsync(string modName)
	{
		IReadOnlyList<FileEntry> screenshots = await fileSystem.ListAsync(DataSubDirectory.ModScreenshots, ModArchiveAccessor.GetModScreenshotsPrefix(modName));
		foreach (string name in screenshots.Select(s => s.Name))
			await fileSystem.DeleteAsync(DataSubDirectory.ModScreenshots, name);
	}

	public async Task MoveScreenshotsAsync(string originalModName, string newModName)
	{
		if (originalModName == newModName)
			return;

		string originalPrefix = ModArchiveAccessor.GetModScreenshotsPrefix(originalModName);
		string newPrefix = ModArchiveAccessor.GetModScreenshotsPrefix(newModName);
		IReadOnlyList<FileEntry> screenshots = await fileSystem.ListAsync(DataSubDirectory.ModScreenshots, originalPrefix);
		foreach (string name in screenshots.Select(s => s.Name))
		{
			byte[] contents = await fileSystem.ReadAllBytesAsync(DataSubDirectory.ModScreenshots, name) ?? throw new InvalidOperationException($"Screenshot '{name}' was listed but could not be read.");
			await fileSystem.WriteAllBytesAsync(DataSubDirectory.ModScreenshots, $"{newPrefix}{name[originalPrefix.Length..]}", contents);
			await fileSystem.DeleteAsync(DataSubDirectory.ModScreenshots, name);
		}
	}
}
