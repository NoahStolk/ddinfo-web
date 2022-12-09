using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class UploadSubmission
{
	public static async Task<bool?> HandleAsync(AddUploadRequest addUploadRequest)
	{
		try
		{
			HttpResponseMessage hrm = await AsyncHandler.Client.SubmitScore(addUploadRequest);
			return hrm.IsSuccessStatusCode; // TODO: Generate API call with actual response.
		}
		catch
		{
			return null;
		}
	}
}
