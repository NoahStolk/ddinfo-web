using DevilDaggersInfo.App.Launcher;

// TODO: Check launcher version and close if outdated, telling the user to update the launcher itself first.

UpdateLogic updateLogic = new();

if (!updateLogic.IsInstalled())
{
	await updateLogic.DownloadAndInstall();
}
else if (await updateLogic.ShouldUpdate())
{
	updateLogic.CleanOldInstallation();
	await updateLogic.DownloadAndInstall();
}

updateLogic.Launch();
