using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Core.ApiClient;

public class AsyncHandler
{
	public AsyncHandler(string currentAppVersionString, ToolBuildType buildType)
	{
		if (!AppVersion.TryParse(currentAppVersionString, out AppVersion? currentAppVersion))
			throw new InvalidOperationException("The current version number is invalid.");

		CurrentAppVersion = currentAppVersion;
		ToolBuildType = buildType;
	}

	public ApiHttpClient Client { get; } = new(new() { BaseAddress = new("https://devildaggers.info") });

	public AppVersion CurrentAppVersion { get; }

	public ToolBuildType ToolBuildType { get; }

	public void Run<TTaskHandler, TResult>(Action<TResult?> callback)
		where TTaskHandler : ITaskHandler<TResult>
	{
		Task.Run(async () => callback(await TTaskHandler.HandleAsync(this)));
	}
}
