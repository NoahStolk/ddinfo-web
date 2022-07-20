using DevilDaggersInfo.Razor.ReplayEditor.Enums;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.State;

public record ReplayEditorState(
	int StartTick,
	int EndTick,
	bool ShowTicksWithoutEvents,
	Dictionary<SwitchableEventType, bool> ShownEventTypes);
