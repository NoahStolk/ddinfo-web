using DevilDaggersInfo.Api.App.CustomLeaderboards;
using System.Net.Http.Json;

namespace DevilDaggersInfo.App.Ui.Base.Networking.TaskHandlers;

public static class UploadSubmission
{
	public static async Task<GetUploadSuccess?> HandleAsync(AddUploadRequest addUploadRequest)
	{
		try
		{
			HttpResponseMessage hrm = await AsyncHandler.Client.SubmitScore(addUploadRequest);

			// TODO: Also return error if the response is not successful.
			return hrm.IsSuccessStatusCode ? await hrm.Content.ReadFromJsonAsync<GetUploadSuccess>() : null;
		}
		catch
		{
			return null;
		}
	}
}
