using DevilDaggersInfo.Razor.CustomLeaderboard.Enums;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature;

public static class RecorderReducer
{
	[ReducerMethod]
	public static RecorderState ReduceUploadRunAction(RecorderState state, UploadRunAction action)
	{
		return state with { State = RecorderStateType.Uploading };
	}

	[ReducerMethod]
	public static RecorderState ReduceUploadRunSuccessAction(RecorderState state, UploadRunSuccessAction action)
	{
		return new(RecorderStateType.CompletedUpload, state.Marker, null, action.UploadSuccess, DateTime.Now);
	}

	[ReducerMethod]
	public static RecorderState ReduceUploadRunFailureAction(RecorderState state, UploadRunFailureAction action)
	{
		return new(RecorderStateType.CompletedUpload, state.Marker, action.Error, null, state.LastSuccessfulUpload);
	}
}
