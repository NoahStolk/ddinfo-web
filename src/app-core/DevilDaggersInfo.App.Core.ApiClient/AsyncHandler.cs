using DevilDaggersInfo.App.Core.ApiClient.ApiClients;
using DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;

namespace DevilDaggersInfo.App.Core.ApiClient;

public static class AsyncHandler
{
	public static ApiHttpClient Client { get; } = new(new() { BaseAddress = new("https://devildaggers.info") });

	public static DdclApiHttpClient DdclClient { get; } = new(new() { BaseAddress = new("https://devildaggers.info") });

	public static DdseApiHttpClient DdseClient { get; } = new(new() { BaseAddress = new("https://devildaggers.info") });

	public static void Run<TResult>(Action<TResult?> callback, Func<Task<TResult?>> call)
	{
		Task.Run(async () => callback(await call()));
	}
}
