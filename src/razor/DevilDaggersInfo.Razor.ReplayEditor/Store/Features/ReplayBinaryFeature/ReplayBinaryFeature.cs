using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Razor.ReplayEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayBinaryFeature;

public class ReplayBinaryFeature : Feature<ReplayBinaryState>
{
	public override string GetName() => "Replay binary";

	protected override ReplayBinaryState GetInitialState()
	{
		ReplayBinary replayBinary = ReplayBinary.CreateDefault();
		return new(replayBinary, "(Untitled)");
	}
}
