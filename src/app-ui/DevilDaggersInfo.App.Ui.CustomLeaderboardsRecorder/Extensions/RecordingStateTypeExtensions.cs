using DevilDaggersInfo.Common.Exceptions;

namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.Extensions;

public static class RecordingStateTypeExtensions
{
	public static string ToDisplayString(this RecordingStateType recordingStateType) => recordingStateType switch
	{
		RecordingStateType.WaitingForGame => "Waiting for game",
		RecordingStateType.Recording => "Recording",
		RecordingStateType.WaitingForRestart => "Waiting for restart",
		RecordingStateType.WaitingForLocalReplay => "Waiting for local replay",
		RecordingStateType.WaitingForLeaderboardReplay => "Waiting for leaderboard replay",
		RecordingStateType.WaitingForStats => "Waiting for stats",
		RecordingStateType.WaitingForReplay => "Waiting for replay",
		RecordingStateType.Uploading => "Uploading",
		RecordingStateType.CompletedUpload => "Completed upload",
		_ => throw new InvalidEnumConversionException(recordingStateType),
	};
}
