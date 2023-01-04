using DevilDaggersInfo.App.Ui.Base.StateManagement.Base.States;
using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.States;

namespace DevilDaggersInfo.App.Ui.Base.StateManagement;

/// <summary>
/// <para>
/// Contains a temporary copy of all the states, which is passed to the <see cref="IAction.Reduce"/> method of <see cref="IAction"/>.
/// </para>
/// <para>
/// Actions can then modify the state, which happens in the <see cref="StateManager.ReduceAll"/> method of <see cref="StateManager"/>.
/// </para>
/// <para>
/// The next actions then re-use the modified state, until everything has been reduced.
/// </para>
/// <para>
/// At the end, the modified state will be used to update the actual state which is stored in the <see cref="StateManager"/>. After that, the event handlers act on the new state.
/// </para>
/// </summary>
public record StateReducer
{
	public LayoutState LayoutState { get; set; } = StateManager.LayoutState;
	public ActiveSpawnsetState ActiveSpawnsetState { get; set; } = StateManager.ActiveSpawnsetState;
	public LeaderboardListState LeaderboardListState { get; set; } = StateManager.LeaderboardListState;
	public MarkerState MarkerState { get; set; } = StateManager.MarkerState;
	public RecordingState RecordingState { get; set; } = StateManager.RecordingState;
	public ReplaySceneState ReplaySceneState { get; set; } = StateManager.ReplaySceneState;
	public ArenaEditorState ArenaEditorState { get; set; } = StateManager.ArenaEditorState;
	public SpawnEditorState SpawnEditorState { get; set; } = StateManager.SpawnEditorState;
	public SpawnsetHistoryState SpawnsetHistoryState { get; set; } = StateManager.SpawnsetHistoryState;
	public SpawnsetState SpawnsetState { get; set; } = StateManager.SpawnsetState;
}
