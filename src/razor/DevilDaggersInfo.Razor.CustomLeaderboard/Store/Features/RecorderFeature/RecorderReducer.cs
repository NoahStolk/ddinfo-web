using DevilDaggersInfo.Razor.CustomLeaderboard.Enums;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature.Actions;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature;

public static class RecorderReducer
{
	[ReducerMethod]
	public static RecorderState ReduceFetchMarkerAction(RecorderState state, FetchMarkerAction action)
	{
		return new(RecorderStateType.WaitingForMarker, null, state.UploadError, state.UploadSuccess, state.LastSuccessfulUpload);
	}

	[ReducerMethod]
	public static RecorderState ReduceFetchMarkerSuccessAction(RecorderState state, FetchMarkerSuccessAction action)
	{
		return new(RecorderStateType.Recording, action.Marker, state.UploadError, state.UploadSuccess, state.LastSuccessfulUpload);
	}

	[ReducerMethod]
	public static RecorderState ReduceFetchMarkerFailureAction(RecorderState state, FetchMarkerFailureAction action)
	{
		return new(RecorderStateType.WaitingForGame, null, state.UploadError, state.UploadSuccess, state.LastSuccessfulUpload);
	}

	[ReducerMethod]
	public static RecorderState ReduceUploadRunAction(RecorderState state, UploadRunAction action)
	{
		return new(RecorderStateType.Uploading, state.Marker, state.UploadError, state.UploadSuccess, state.LastSuccessfulUpload);
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
