namespace DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.Actions;

public record SetFailedUpload(string ErrorMessage) : IAction
{
	public void Reduce(StateReducer stateReducer)
	{
		stateReducer.UploadSuccessState = new(null, ErrorMessage);
	}
}
