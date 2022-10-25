using DevilDaggersInfo.Api.Main.Tools;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Core.ApiClient;

public class AsyncHandler
{
	private static readonly ApiHttpClient _client = new(new() { BaseAddress = new("https://devildaggers.info") });

	private readonly AppVersion _currentAppVersion;
	private readonly ToolBuildType _buildType;

	public AsyncHandler(string currentAppVersionString, ToolBuildType buildType)
	{
		if (!AppVersion.TryParse(currentAppVersionString, out AppVersion? currentAppVersion))
			throw new InvalidOperationException("The current version number is invalid.");

		_currentAppVersion = currentAppVersion;
		_buildType = buildType;
	}

	public void CheckForUpdates(Action<AppVersion?> callback)
	{
		Task.Run(async () => callback(await new FetchLatestDistribution(_currentAppVersion, _buildType).HandleAsync()));
	}

	private sealed record FetchLatestDistribution(AppVersion CurrentAppVersion, ToolBuildType BuildType)
	{
		public async Task<AppVersion?> HandleAsync()
		{
			try
			{
				GetToolDistribution distribution = await _client.GetLatestToolDistribution("ddinfo-tools", ToolPublishMethod.SelfContained, BuildType);
				if (!AppVersion.TryParse(distribution.VersionNumber, out AppVersion? onlineVersion))
					return null;

				return onlineVersion > CurrentAppVersion ? onlineVersion : null;
			}
			catch
			{
				return null;
			}
		}
	}
}
