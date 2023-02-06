using DevilDaggersInfo.App.Launcher;

// TODO: Check launcher version and close if outdated, telling the user to update the launcher itself first.

if (!UpdateLogic.IsInstalled())
{
	Cmd.WriteLine(ConsoleColor.Yellow, "Downloading...");
	await UpdateLogic.DownloadAndInstall();
	Cmd.WriteLine(ConsoleColor.Green, "App installed.");
}
else if (await UpdateLogic.ShouldUpdate())
{
	UpdateLogic.CleanOldInstallation();

	Cmd.WriteLine(ConsoleColor.Yellow, "Downloading new version...");
	await UpdateLogic.DownloadAndInstall();
	Cmd.WriteLine(ConsoleColor.Green, "Update installed.");
}

UpdateLogic.Launch();
