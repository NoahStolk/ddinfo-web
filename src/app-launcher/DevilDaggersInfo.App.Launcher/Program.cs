using DevilDaggersInfo.App.Launcher;
using System.Security.Principal;

// TODO: Check launcher version and close if outdated, telling the user to update the launcher itself first.

WindowsIdentity identity = WindowsIdentity.GetCurrent();
WindowsPrincipal principal = new(identity);
if (!principal.IsInRole(WindowsBuiltInRole.Administrator))
{
	Cmd.WriteLine(ConsoleColor.Red, "The launcher needs access to the Program Files directory to install the app. Please run it as administrator.");
	Cmd.WriteLine(ConsoleColor.Gray, "Press any key to exit...");
	Console.ReadLine();
	return;
}

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
