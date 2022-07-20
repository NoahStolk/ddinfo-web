using DevilDaggersInfo.Razor.ReplayEditor.Enums;
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
			ShowTicksWithoutEvents: true,
			ShownEventTypes: Enum.GetValues<SwitchableEventType>().ToDictionary(e => e, _ => true));
	}
}
