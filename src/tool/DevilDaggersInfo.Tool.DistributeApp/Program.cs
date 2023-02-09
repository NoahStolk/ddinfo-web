using DevilDaggersInfo.Api.Main.Authentication;
using DevilDaggersInfo.Tool.DistributeApp;
using DevilDaggersInfo.Types.Web;

Console.WriteLine("Distribute app? y/n");
if (Console.ReadKey().KeyChar == 'y')
	await DistributeAppAsync();

Console.WriteLine("Distribute app launcher? y/n");
if (Console.ReadKey().KeyChar == 'y')
	await DistributeLauncherAsync();

static async Task DistributeLauncherAsync()
{
	const string toolName = "ddinfo-tools-launcher";
	string root = Path.Combine("..", "..", "..", "..", "..");
	string projectFilePath = Path.Combine(root, "app-launcher", "DevilDaggersInfo.App.Launcher", "DevilDaggersInfo.App.Launcher.csproj");
	string zipOutputDirectory = Path.Combine(root, "app-launcher", "DevilDaggersInfo.App.Launcher", "bin");

	await BuildAndUploadAsync(toolName, projectFilePath, zipOutputDirectory, ToolBuildType.WindowsConsole, ToolPublishMethod.Aot);
	await BuildAndUploadAsync(toolName, projectFilePath, zipOutputDirectory, ToolBuildType.LinuxConsole, ToolPublishMethod.Aot);
}

static async Task DistributeAppAsync()
{
	const string toolName = "ddinfo-tools";
	string root = Path.Combine("..", "..", "..", "..", "..");
	string projectFilePath = Path.Combine(root, "app", "DevilDaggersInfo.App", "DevilDaggersInfo.App.csproj");
	string zipOutputDirectory = Path.Combine(root, "app", "DevilDaggersInfo.App", "bin");

	await BuildAndUploadAsync(toolName, projectFilePath, zipOutputDirectory, ToolBuildType.WindowsWarp, ToolPublishMethod.SelfContained);
	await BuildAndUploadAsync(toolName, projectFilePath, zipOutputDirectory, ToolBuildType.LinuxWarp, ToolPublishMethod.SelfContained);
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
