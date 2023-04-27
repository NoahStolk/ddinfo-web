using CliWrap;
using DevilDaggersInfo.Api.Admin.Tools;
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

		Dictionary<string, string> dictionary = new()
		{
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

		switch (toolPublishMethod)
		{
			case ToolPublishMethod.SelfContained:
				dictionary.Add("PublishSingleFile", "True");
				dictionary.Add("SelfContained", "True");
				dictionary.Add("PublishMethod", "SELF_CONTAINED");
				break;
			case ToolPublishMethod.Aot:
				dictionary.Add("PublishAot", "True");
				break;
			case ToolPublishMethod.Default: throw new NotImplementedException();
			default: throw new ArgumentOutOfRangeException(nameof(toolPublishMethod), toolPublishMethod, null);
		}

		StringBuilder stringBuilder = new($"publish {projectFilePath}");
		foreach (KeyValuePair<string, string> keyValuePair in dictionary)
			stringBuilder.Append(" -p:").Append(keyValuePair.Key).Append('=').Append(keyValuePair.Value);

		return stringBuilder.ToString();
	}
}
