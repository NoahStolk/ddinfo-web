using DevilDaggersInfo.Api.App.Updates;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;
using System.Diagnostics;
using System.IO.Compression;

// TODO: Check launcher version and close if outdated.

string installationDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "DDINFO");

#if WINDOWS
const ToolBuildType toolBuildType = ToolBuildType.WindowsWarp;
#elif LINUX
const ToolBuildType toolBuildType = ToolBuildType.LinuxWarp;
#endif

// TODO: Get version from installed app, or null.
bool installed = false;
AppVersion currentVersion = new(0, 0, 0);
if (installed)
{
	if (await ShouldUpdate())
	{
		foreach (string file in Directory.GetFiles(installationDirectory))
			File.Delete(file);

		await DownloadAndInstall();
	}
}
else
{
	await DownloadAndInstall();
}

Launch();

async Task<bool> ShouldUpdate()
{
	Console.WriteLine("Checking for updates...");
	GetLatestVersion latestVersion = await AsyncHandler.Client.GetLatestVersion(ToolPublishMethod.SelfContained, toolBuildType);
	if (!AppVersion.TryParse(latestVersion.VersionNumber, out AppVersion? onlineVersion))
	{
		Console.WriteLine($"Could not parse '{latestVersion.VersionNumber}' as version number.");
		return false;
	}

	if (onlineVersion > currentVersion)
	{
		Console.WriteLine($"New version available: {onlineVersion}");
		return true;
	}

	Console.WriteLine("No new version available.");
	return false;
}

async Task DownloadAndInstall()
{
	GetLatestVersionFile file = await AsyncHandler.Client.GetLatestVersionFile(ToolPublishMethod.SelfContained, toolBuildType);
	using MemoryStream ms = new(file.ZipFileContents);
	using ZipArchive archive = new(ms);
	archive.ExtractToDirectory(installationDirectory, true);
}

void Launch()
{
	// TODO: Use something else on Linux.
	string? newExecutableFileName = Array.Find(Directory.GetFiles(installationDirectory), f => Path.GetExtension(f) == ".exe");
	if (newExecutableFileName == null)
		throw new($"Could not launch new version. Please launch it manually. It is installed at {installationDirectory}.");

	Process process = new();
	process.StartInfo.FileName = newExecutableFileName;
	process.StartInfo.WorkingDirectory = installationDirectory;
	process.Start();
}
