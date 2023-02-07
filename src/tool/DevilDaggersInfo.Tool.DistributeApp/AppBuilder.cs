using CliWrap;
using DevilDaggersInfo.Types.Web;
using System.Text;

namespace DevilDaggersInfo.Tool.DistributeApp;

public static class AppBuilder
{
	public static async Task BuildAsync(string projectFilePath, string publishDirectoryName, ToolBuildType toolBuildType, ToolPublishMethod toolPublishMethod)
	{
		await Cli.Wrap("dotnet")
			.WithArguments(GetArguments(projectFilePath, publishDirectoryName, toolBuildType, toolPublishMethod))
			.ExecuteAsync();
	}

	private static string GetArguments(string projectFilePath, string publishDirectoryName, ToolBuildType toolBuildType, ToolPublishMethod toolPublishMethod)
	{
		string runtimeIdentifier = toolBuildType switch
		{
			ToolBuildType.LinuxWarp or ToolBuildType.LinuxConsole => "linux-x64",
			ToolBuildType.WindowsWarp or ToolBuildType.WindowsConsole => "win-x64",
			_ => throw new NotImplementedException(),
		};

		bool isSelfContained = toolPublishMethod == ToolPublishMethod.SelfContained;

		Dictionary<string, string> dictionary = new()
		{
			{ "PublishSingleFile", isSelfContained.ToString() },
			{ "SelfContained", isSelfContained.ToString() },
			{ "PublishTrimmed", "True" },
			{ "EnableCompressionInSingleFile", "True" },
			{ "PublishReadyToRun", "False" },
			{ "PublishProtocol", "FileSystem" },
			{ "TargetFramework", "net7.0" },
			{ "RuntimeIdentifier", runtimeIdentifier },
			{ "Platform", "x64" },
			{ "Configuration", "Release" },
			{ "PublishDir", publishDirectoryName },
		};

		if (isSelfContained)
			dictionary.Add("PublishMethod", "SELF_CONTAINED");

		StringBuilder stringBuilder = new($"publish {projectFilePath}");
		foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			stringBuilder.Append(" -p:").Append(keyValuePair.Key).Append('=').Append(keyValuePair.Value);

		return stringBuilder.ToString();
	}
}
