using DevilDaggersInfo.Api.App.Updates;
using DevilDaggersInfo.App.Ui;
using System.Diagnostics;
using System.IO.Compression;

namespace DevilDaggersInfo.App;

public static class UpdateLogic
{
#if WINDOWS
	private const ToolBuildType _appBuildType = ToolBuildType.WindowsWarp;
	private const string _exeFileName = "ddinfo-tools.exe"; // TODO: Get this from the AssemblyName MSBuild property.
	private const string _oldExeFileName = "ddinfo-tools-old.exe";
#elif LINUX
	private const ToolBuildType _appBuildType = ToolBuildType.LinuxWarp;
	private const string _exeFileName = "ddinfo-tools";
	private const string _oldExeFileName = "ddinfo-tools-old";
#endif

	private static readonly Uri _baseAddress = new("https://devildaggers.info/");

	/// <summary>
	/// Can be called when the application starts up.
	/// </summary>
	public static void TryDeleteOldExecutableOnAppStart()
	{
		try
		{
			if (File.Exists(_oldExeFileName))
				File.Delete(_oldExeFileName);
		}
		catch (UnauthorizedAccessException ex)
		{
			Root.Log.Error(ex, "Could not delete old executable on start up (probably because the old executable is currently running).");
		}
	}

	public static async Task RunAsync()
	{
		try
		{
			try
			{
				// If a previous attempt to update failed, delete the old executable.
				if (File.Exists(_oldExeFileName))
				{
					UpdateWindow.LogMessages.Add("Deleting old executable...");
					File.Delete(_oldExeFileName);
				}

				// Rename the currently running executable, so the new executable can be installed.
				UpdateWindow.LogMessages.Add("Renaming old executable...");
				File.Move(_exeFileName, _oldExeFileName);
			}
			catch (UnauthorizedAccessException ex)
			{
				const string errorMessage = "Could not delete or rename the old executable, probably because it is currently in use.";
				UpdateWindow.LogMessages.Add($"{errorMessage}\n{ex.Message}");
				Root.Log.Error(ex, errorMessage);
			}

			UpdateWindow.LogMessages.Add("Downloading update...");
			byte[] zipFileContents = await DownloadAppAsync();

			UpdateWindow.LogMessages.Add("Opening ZIP...");

			using MemoryStream ms = new(zipFileContents);
			using ZipArchive archive = new(ms);

			UpdateWindow.LogMessages.Add("Installing update...");

			// This will overwrite all the existing files.
			archive.ExtractToDirectory(AssemblyUtils.InstallationDirectory, true);

			UpdateWindow.LogMessages.Add("Launching new version...");
			Process process = new();
			process.StartInfo.FileName = _exeFileName;
			process.StartInfo.WorkingDirectory = AssemblyUtils.InstallationDirectory;
			process.Start();

			Environment.Exit(0);
		}
		catch (Exception ex)
		{
			const string errorMessage = "An error occurred while updating the application.";
			UpdateWindow.LogMessages.Add($"{errorMessage}\n{ex.Message}");
			Root.Log.Error(ex, errorMessage);
		}
	}

	private static async Task<byte[]> DownloadAppAsync()
	{
		using HttpRequestMessage request = new()
		{
			RequestUri = new($"api/app/updates/latest-version-file?publishMethod={ToolPublishMethod.SelfContained}&buildType={_appBuildType}", UriKind.Relative),
			Method = HttpMethod.Get,
		};

		using HttpClient httpClient = new() { BaseAddress = _baseAddress };

		using HttpResponseMessage response = await httpClient.SendAsync(request);
		if (!response.IsSuccessStatusCode)
			throw new HttpRequestException(await response.Content.ReadAsStringAsync(), null, response.StatusCode);

		return await response.Content.ReadAsByteArrayAsync();
	}
}
