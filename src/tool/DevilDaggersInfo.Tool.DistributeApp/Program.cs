using DevilDaggersInfo.Api.Main.Authentication;
using DevilDaggersInfo.Tool.DistributeApp;
using DevilDaggersInfo.Types.Web;

Console.WriteLine("Distribute app for Windows? y/n");
if (Console.ReadKey().KeyChar == 'y')
	await DistributeAsync("ddinfo-tools", "app", "DevilDaggersInfo.App", ToolBuildType.WindowsWarp, ToolPublishMethod.SelfContained);

Console.WriteLine("Distribute app for Linux? y/n");
if (Console.ReadKey().KeyChar == 'y')
	await DistributeAsync("ddinfo-tools", "app", "DevilDaggersInfo.App", ToolBuildType.LinuxWarp, ToolPublishMethod.SelfContained);

Console.WriteLine("Distribute app launcher for Windows? y/n");
if (Console.ReadKey().KeyChar == 'y')
	await DistributeAsync("ddinfo-tools-launcher", "app-launcher", "DevilDaggersInfo.App.Launcher", ToolBuildType.WindowsConsole, ToolPublishMethod.Aot);

Console.WriteLine("Distribute app launcher for Linux? y/n");
if (Console.ReadKey().KeyChar == 'y')
	await DistributeAsync("ddinfo-tools-launcher", "app-launcher", "DevilDaggersInfo.App.Launcher", ToolBuildType.LinuxConsole, ToolPublishMethod.Aot);

static async Task DistributeAsync(string toolName, string srcDir, string projectName, ToolBuildType toolBuildType, ToolPublishMethod toolPublishMethod)
{
	string root = Path.Combine("..", "..", "..", "..", "..");
	string projectFilePath = Path.Combine(root, srcDir, projectName, $"{projectName}.csproj");
	string zipOutputDirectory = Path.Combine(root, srcDir, projectName, "bin");
	await BuildAndUploadAsync(toolName, projectFilePath, zipOutputDirectory, toolBuildType, toolPublishMethod);
}

static async Task BuildAndUploadAsync(string toolName, string projectFilePath, string zipOutputDirectory, ToolBuildType toolBuildType, ToolPublishMethod toolPublishMethod)
{
	const string publishDirectoryName = "_temp-release";

	Console.WriteLine("Building...");
	await AppBuilder.BuildAsync(projectFilePath, publishDirectoryName, toolBuildType, toolPublishMethod);

	string publishDirectoryPath = Path.Combine(Path.GetDirectoryName(projectFilePath) ?? throw new($"Cannot get root directory of {projectFilePath}."), publishDirectoryName);

	// Copy content file if it exists.
	string contentFilePath = Path.Combine(projectFilePath, "..", "bin", "Debug", "net7.0", "ddinfo");
	if (File.Exists(contentFilePath))
		File.Copy(contentFilePath, Path.Combine(publishDirectoryPath, "ddinfo"));

	// Zip build and content file.
	string version = ProjectReader.ReadVersionFromProjectFile(projectFilePath);
	string outputZipFilePath = Path.Combine(zipOutputDirectory, $"{toolName}-{version}-{toolBuildType}-{toolPublishMethod}.zip");
	ZipWriter.ZipAndDelete(outputZipFilePath, publishDirectoryPath);

	LoginResponse loginToken = await ApiHttpClient.LoginAsync();
	await ApiHttpClient.UploadAsync(toolName, version, toolBuildType, toolPublishMethod, outputZipFilePath, loginToken.Token);

	Console.WriteLine($"Deleting zip file '{outputZipFilePath}'");
	File.Delete(outputZipFilePath);
}
