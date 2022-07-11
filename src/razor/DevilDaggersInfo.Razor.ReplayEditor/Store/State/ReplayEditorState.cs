namespace DevilDaggersInfo.Razor.ReplayEditor.Store.State;

public record ReplayEditorState(int StartTick, int EndTick, List<int> OpenedTicks);
