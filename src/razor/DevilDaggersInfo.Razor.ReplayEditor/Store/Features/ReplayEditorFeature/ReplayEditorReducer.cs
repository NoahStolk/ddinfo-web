using DevilDaggersInfo.Razor.ReplayEditor.Enums;
using DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditorFeature.Actions;
using DevilDaggersInfo.Razor.ReplayEditor.Store.State;
using Fluxor;

namespace DevilDaggersInfo.Razor.ReplayEditor.Store.Features.ReplayEditorFeature;

public static class ReplayEditorReducer
{
	[ReducerMethod]
	public static ReplayEditorState ReduceSelectTickRangeAction(ReplayEditorState state, SelectTickRangeAction action)
	{
		int startTick = Math.Max(0, action.StartTick);
		int endTick = Math.Max(action.EndTick, startTick + 1);

		return state with
		{
			StartTick = startTick,
			EndTick = endTick,
		};
	}

	[ReducerMethod]
	public static ReplayEditorState ReduceToggleTickAction(ReplayEditorState state, ToggleTickAction action)
	{
		List<int> openedTicks = new(state.OpenedTicks);
		if (openedTicks.Contains(action.Tick))
			openedTicks.Remove(action.Tick);
		else
			openedTicks.Add(action.Tick);

		return state with { OpenedTicks = openedTicks };
	}

	[ReducerMethod]
	public static ReplayEditorState ReduceToggleShowTicksWithoutEventsAction(ReplayEditorState state, ToggleShowTicksWithoutEventsAction action)
	{
		return state with { ShowTicksWithoutEvents = !state.ShowTicksWithoutEvents };
	}

	[ReducerMethod]
	public static ReplayEditorState ReduceToggleShowEventTypesAction(ReplayEditorState state, ToggleShowEventTypeAction action)
	{
		Dictionary<SwitchableEventType, bool> shownEventTypes = new(state.ShownEventTypes);
		if (!shownEventTypes.ContainsKey(action.SwitchableEventType))
			shownEventTypes.Add(action.SwitchableEventType, true); // TODO: Probably log a warning here.

		shownEventTypes[action.SwitchableEventType] = !shownEventTypes[action.SwitchableEventType];

		return state with { ShownEventTypes = shownEventTypes };
	}

	[ReducerMethod]
	public static ReplayEditorState ReduceToggleAllEvents(ReplayEditorState state, ToggleAllEventsAction action)
	{
		if (action.Show)
			return state with { OpenedTicks = Enumerable.Range(state.StartTick, state.EndTick - state.StartTick).ToList() };

		return state with { OpenedTicks = new() };
	}
}
