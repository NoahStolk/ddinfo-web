using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;

public record UploadSuccessState(GetUploadResponse? UploadResponse)
{
	public static UploadSuccessState GetDefault()
	{
		return new((GetUploadResponse?)null);
	}
}
