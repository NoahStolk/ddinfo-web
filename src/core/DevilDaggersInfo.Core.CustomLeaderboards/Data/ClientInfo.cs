using DevilDaggersInfo.Api.Ddcl.Tools;

namespace DevilDaggersInfo.Core.CustomLeaderboards.Data;

// TODO: Use IOptions.
public record ClientInfo(
	string OperatingSystem,
	string ApplicationName,
	string ApplicationVersion,
	string ApplicationBuildMode,
	ToolPublishMethod PublishMethod,
	ToolBuildType BuildType);
