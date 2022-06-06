using DevilDaggersInfo.Api.Ddcl.Tools;

namespace DevilDaggersInfo.Core.CustomLeaderboard.Configuration;

public record ClientOptions(
	string OperatingSystem,
	string ApplicationName,
	string ApplicationVersion,
	string ApplicationBuildMode,
	ToolPublishMethod PublishMethod,
	ToolBuildType BuildType);
