using DevilDaggersInfo.Api.App.CustomLeaderboards;
using System.Net.Http.Json;

namespace DevilDaggersInfo.App.Ui.Base.Networking.TaskHandlers;

public static class UploadSubmission
{
	public static async Task<ResultWrapper> HandleAsync(AddUploadRequest addUploadRequest)
	{
		HttpResponseMessage hrm = await AsyncHandler.Client.SubmitScore(addUploadRequest);

		if (!hrm.IsSuccessStatusCode)
			return new() { Error = await hrm.Content.ReadAsStringAsync() };

		return new() { Result = await hrm.Content.ReadFromJsonAsync<GetUploadSuccess>() };
	}

	public class ResultWrapper
	{
		public GetUploadSuccess? Result { get; init; }

		public string? Error { get; init; }
	}
}
