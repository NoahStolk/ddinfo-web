using DevilDaggersInfo.Api.App.CustomLeaderboards;
using System.Net.Http.Json;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class UploadSubmission
{
	public static async Task<GetUploadSuccess?> HandleAsync(AddUploadRequest addUploadRequest)
	{
		try
		{
			HttpResponseMessage hrm = await AsyncHandler.Client.SubmitScore(addUploadRequest);
			return hrm.IsSuccessStatusCode ? await hrm.Content.ReadFromJsonAsync<GetUploadSuccess>() : null;
		}
		catch
		{
			return null;
		}
	}
}
