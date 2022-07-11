using DevilDaggersInfo.Razor.ReplayEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditorFeature;

public class ReplayEditorFeature : Feature<ReplayEditorState>
{
	public override string GetName() => "Replay editor";

	protected override ReplayEditorState GetInitialState()
	{
		return new(
			StartTick: 0,
			EndTick: 60,
			OpenedTicks: new());
	}
}
