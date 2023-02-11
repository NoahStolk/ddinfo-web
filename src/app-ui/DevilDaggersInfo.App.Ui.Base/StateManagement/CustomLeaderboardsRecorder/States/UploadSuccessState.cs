using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;

public record UploadSuccessState(GetUploadSuccess? UploadSuccess, string? UploadError)
{
	public static UploadSuccessState GetDefault()
	{
		return new(null, null);
	}
}
