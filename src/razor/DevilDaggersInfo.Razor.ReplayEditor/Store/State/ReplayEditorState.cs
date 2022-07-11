using DevilDaggersInfo.Core.Replay;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.State;

public record ReplayEditorState(
	ReplayBinary ReplayBinary,
	string Name,
	int StartTick,
	int EndTick);
