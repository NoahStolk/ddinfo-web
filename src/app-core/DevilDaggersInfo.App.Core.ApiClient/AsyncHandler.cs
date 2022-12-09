namespace DevilDaggersInfo.App.Core.ApiClient;

public static class AsyncHandler
{
	public static AppApiHttpClient Client { get; } = new(new() { BaseAddress = new("https://devildaggers.info") });

	public static void Run<TResult>(Action<TResult?> callback, Func<Task<TResult?>> call)
	{
		Task.Run(async () => callback(await call()));
	}
}
