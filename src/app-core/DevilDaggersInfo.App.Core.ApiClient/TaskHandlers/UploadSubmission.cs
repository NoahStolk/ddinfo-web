using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

namespace DevilDaggersInfo.App.Core.ApiClient.TaskHandlers;

public static class UploadSubmission
{
	public static async Task<bool?> HandleAsync(AddUploadRequest addUploadRequest)
	{
		try
		{
			HttpResponseMessage hrm = await AsyncHandler.DdclClient.SubmitScoreForDdcl(addUploadRequest);
			return hrm.IsSuccessStatusCode; // TODO: Generate API call with actual response.
		}
		catch
		{
			return null;
		}
	}
}
