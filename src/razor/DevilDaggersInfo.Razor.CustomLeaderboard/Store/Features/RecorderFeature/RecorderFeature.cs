using DevilDaggersInfo.Razor.CustomLeaderboard.Enums;
using DevilDaggersInfo.Razor.CustomLeaderboard.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Store.Features.RecorderFeature;

public class RecorderFeature : Feature<RecorderState>
{
	public override string GetName() => "Recorder";

	protected override RecorderState GetInitialState() => new(RecorderStateType.WaitingForGame, null, null, null);
}
