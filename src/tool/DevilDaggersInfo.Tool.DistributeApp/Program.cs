using DevilDaggersInfo.Api.Main.Authentication;
using DevilDaggersInfo.Tool.DistributeApp;
using DevilDaggersInfo.Types.Web;

await DistributeLauncherAsync();

static async Task DistributeLauncherAsync()
{
	const string toolName = "ddinfo-tools-launcher";
	const string projectFilePath = """C:\Users\NOAH\source\repos\DevilDaggersInfo\src\app-launcher\DevilDaggersInfo.App.Launcher\DevilDaggersInfo.App.Launcher.csproj""";
	const string zipOutputDirectory = """C:\Users\NOAH\source\repos\DevilDaggersInfo\src\app-launcher\DevilDaggersInfo.App.Launcher\bin""";

	await BuildAndUploadAsync(toolName, projectFilePath, zipOutputDirectory, ToolBuildType.WindowsConsole, ToolPublishMethod.SelfContained);
	await BuildAndUploadAsync(toolName, projectFilePath, zipOutputDirectory, ToolBuildType.LinuxConsole, ToolPublishMethod.SelfContained);
}

static async Task DistributeAppAsync()
{
	const string toolName = "ddinfo-tools";
	const string projectFilePath = """C:\Users\NOAH\source\repos\DevilDaggersInfo\src\app\DevilDaggersInfo.App\DevilDaggersInfo.App.csproj""";
	const string zipOutputDirectory = """C:\Users\NOAH\source\repos\DevilDaggersInfo\src\app\DevilDaggersInfo.App\bin""";

	await BuildAndUploadAsync(toolName, projectFilePath, zipOutputDirectory, ToolBuildType.WindowsWarp, ToolPublishMethod.SelfContained);
	await BuildAndUploadAsync(toolName, projectFilePath, zipOutputDirectory, ToolBuildType.LinuxWarp, ToolPublishMethod.SelfContained);
}

static async Task BuildAndUploadAsync(string toolName, string projectFilePath, string zipOutputDirectory, ToolBuildType toolBuildType, ToolPublishMethod toolPublishMethod)
{
	const string publishDirectoryName = "_temp-release";

	await AppBuilder.BuildAsync(projectFilePath, publishDirectoryName, toolBuildType, toolPublishMethod);

	string publishDirectoryPath = Path.Combine(Path.GetDirectoryName(projectFilePath) ?? throw new(), publishDirectoryName);

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

	Console.WriteLine( $"Deleting zip file '{outputZipFilePath}'");
	File.Delete(outputZipFilePath);
}
