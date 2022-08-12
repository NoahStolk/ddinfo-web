using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecordingFeature;

public class RecordingFeature : Feature<RecordingState>
{
	public override string GetName() => "Recording";

	protected override RecordingState GetInitialState() => new(false);
}
