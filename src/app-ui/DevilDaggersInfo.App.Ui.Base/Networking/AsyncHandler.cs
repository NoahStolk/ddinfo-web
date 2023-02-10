using DevilDaggersInfo.App.Ui.Base.DependencyPattern;
using Serilog.Events;

namespace DevilDaggersInfo.App.Ui.Base.Networking;

public static class AsyncHandler
{
	public static AppApiHttpClient Client { get; } = new(new() { BaseAddress = new("https://devildaggers.info") });

	public static void Run<TResult>(Action<TResult?> callback, Func<Task<TResult>> call)
	{
		Task.Run(async () => callback(await TryCall()));

		async Task<TResult?> TryCall()
		{
			try
			{
				return await call();
			}
			catch (Exception ex)
			{
				Root.Dependencies.NativeDialogService.ReportError("API error", "API call failed.", ex);
				Root.Dependencies.Log.Write(LogEventLevel.Error, ex, "API error");
				return default;
			}
		}
	}
}
