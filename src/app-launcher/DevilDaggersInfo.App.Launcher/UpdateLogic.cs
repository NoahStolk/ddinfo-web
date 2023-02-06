using DevilDaggersInfo.Api.App.Updates;
using DevilDaggersInfo.App.Core.ApiClient;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;
using System.Diagnostics;
using System.IO.Compression;

namespace DevilDaggersInfo.App.Launcher;

public static class UpdateLogic
{
#if WINDOWS
	private const ToolBuildType _toolBuildType = ToolBuildType.WindowsWarp;
#elif LINUX
	private const ToolBuildType _toolBuildType = ToolBuildType.LinuxWarp;
#endif

	private const string _installationDirectory = "DDINFO TOOLS";

	public static async Task<bool> ShouldUpdate()
	{
		Cmd.WriteLine(ConsoleColor.Yellow, "Checking for updates...");
		GetLatestVersion latestVersion = await AsyncHandler.Client.GetLatestVersion(ToolPublishMethod.SelfContained, _toolBuildType);
		if (!AppVersion.TryParse(latestVersion.VersionNumber, out AppVersion? onlineVersion))
		{
			Cmd.WriteLine(ConsoleColor.Red, $"Could not parse response from API as a valid version number: '{latestVersion.VersionNumber}'");
			return false;
		}

		string executableFileName = FindExecutableFileName() ?? throw new InvalidOperationException("ShouldUpdate can only be called when the app is installed.");
		FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(executableFileName);
		if (versionInfo.FileVersion == null)
		{
			Cmd.WriteLine(ConsoleColor.Red, "Could not get file version from installed executable.");
			return true;
		}

		AppVersion currentVersion = AppVersion.Parse(versionInfo.FileVersion);
		if (onlineVersion > currentVersion)
		{
			Cmd.WriteLine(ConsoleColor.Green, $"New version available: {onlineVersion}");
			return true;
		}

		Cmd.WriteLine(ConsoleColor.Green, "No new version available.");
		return false;
	}

	public static void CleanOldInstallation()
	{
		if (!Directory.Exists(_installationDirectory))
			return;

		foreach (string existingFile in Directory.GetFiles(_installationDirectory))
			File.Delete(existingFile);
	}

	public static async Task DownloadAndInstall()
	{
		GetLatestVersionFile latestInstallation = await AsyncHandler.Client.GetLatestVersionFile(ToolPublishMethod.SelfContained, _toolBuildType);
		using MemoryStream ms = new(latestInstallation.ZipFileContents);
		using ZipArchive archive = new(ms);
		archive.ExtractToDirectory(_installationDirectory, true);
	}

	public static void Launch()
	{
		string? executableFileName = FindExecutableFileName();
		if (executableFileName == null)
		{
			Cmd.WriteLine(ConsoleColor.Red, $"Could not launch executable. It should be installed at '{_installationDirectory}'.");
			return;
		}

		Process process = new();
		process.StartInfo.FileName = executableFileName;
		process.StartInfo.WorkingDirectory = _installationDirectory;
		process.Start();
	}

	private static string? FindExecutableFileName()
	{
		// TODO: Use something else on Linux.
		return !Directory.Exists(_installationDirectory) ? null : Array.Find(Directory.GetFiles(_installationDirectory), f => Path.GetExtension(f) == ".exe");
	}

	public static bool IsInstalled()
	{
		return FindExecutableFileName() != null;
	}
}
