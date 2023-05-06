using DevilDaggersInfo.App.Ui.Base.StateManagement.CustomLeaderboardsRecorder.States;

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
	public LeaderboardListState LeaderboardListState { get; set; } = StateManager.LeaderboardListState;
	public ReplaySceneState ReplaySceneState { get; set; } = StateManager.ReplaySceneState;
	public UploadResponseState UploadResponseState { get; set; } = StateManager.UploadResponseState;
}
