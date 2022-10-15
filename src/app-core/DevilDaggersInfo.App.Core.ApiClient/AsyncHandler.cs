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
		DistributionLatest dl = new(_currentAppVersion, _buildType, callback);
		Thread thread = new(dl.SetOnlineAppVersion);
		thread.Start();
	}

	private sealed class DistributionLatest
	{
		private readonly AppVersion _currentAppVersion;
		private readonly ToolBuildType _buildType;
		private readonly Action<AppVersion?> _callback;

		public DistributionLatest(AppVersion currentAppVersion, ToolBuildType buildType, Action<AppVersion?> callback)
		{
			_currentAppVersion = currentAppVersion;
			_buildType = buildType;
			_callback = callback;
		}

		public void SetOnlineAppVersion()
		{
			AppVersion? appVersion = GetDistributionLatestReturn();
			_callback(appVersion);

			AppVersion? GetDistributionLatestReturn()
			{
				try
				{
					GetToolDistribution distribution = _client.GetLatestToolDistribution("ddinfo-tools", ToolPublishMethod.SelfContained, _buildType).Result; // TODO: Ok???
					if (!AppVersion.TryParse(distribution.VersionNumber, out AppVersion? onlineVersion))
						return null;

					return onlineVersion > _currentAppVersion ? onlineVersion : null;
				}
				catch // TODO: Return error.
				{
					return null;
				}
			}
		}
	}
}
