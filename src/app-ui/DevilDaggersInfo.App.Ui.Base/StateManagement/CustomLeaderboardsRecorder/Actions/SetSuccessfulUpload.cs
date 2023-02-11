using DevilDaggersInfo.Api.App.CustomLeaderboards;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetSuccessfulUpload(GetUploadSuccess UploadSuccess) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.UploadSuccessState = new(UploadSuccess, null);
	}
}
