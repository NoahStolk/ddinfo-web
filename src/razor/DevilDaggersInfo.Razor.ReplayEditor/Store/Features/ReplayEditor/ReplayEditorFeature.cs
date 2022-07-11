using DevilDaggersInfo.Core.Replay;
using DevilDaggersInfo.Razor.ReplayEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditor;

public class ReplayEditorFeature : Feature<ReplayEditorState>
{
	public override string GetName() => "Replay editor";

	protected override ReplayEditorState GetInitialState()
	{
		ReplayBinary replayBinary = ReplayBinary.CreateDefault();
		return new(ReplayBinary: replayBinary, Name: "(Untitled)", StartTick: 0, EndTick: 60);
	}
}
