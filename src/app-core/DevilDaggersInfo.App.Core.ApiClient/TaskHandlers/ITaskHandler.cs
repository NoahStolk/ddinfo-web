namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public interface ITaskHandler<TResult>
{
	static abstract Task<TResult?> HandleAsync(AsyncHandler asyncHandler);
}
