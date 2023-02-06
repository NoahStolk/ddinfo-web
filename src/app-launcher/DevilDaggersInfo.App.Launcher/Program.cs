using DevilDaggersInfo.App.Launcher;

// TODO: Check launcher version and close if outdated, telling the user to update the launcher itself first.

if (!UpdateLogic.IsInstalled())
{
	await UpdateLogic.DownloadAndInstall();
}
else if (await UpdateLogic.ShouldUpdate())
{
	UpdateLogic.CleanOldInstallation();
	await UpdateLogic.DownloadAndInstall();
}

UpdateLogic.Launch();
