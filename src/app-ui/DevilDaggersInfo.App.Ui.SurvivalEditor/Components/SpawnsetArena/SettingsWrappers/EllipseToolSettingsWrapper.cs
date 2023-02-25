using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena.SettingsWrappers;

public class EllipseToolSettingsWrapper : AbstractComponent, ISettingsWrapper
{
	public EllipseToolSettingsWrapper(IBounds bounds)
		: base(bounds)
	{
		int y = 0;
		ISettingsWrapper wrapper = this;

		// TODO
		// wrapper.AddSlider("Size", ref y, f => StateManager.Dispatch(new SetArenaRectangleSize((int)f)), 1, 10, 1, StateManager.ArenaRectangleState.Size, "0");
		// wrapper.AddCheckbox("Filled", ref y, b => StateManager.Dispatch(new SetArenaRectangleFilled(b)), StateManager.ArenaRectangleState.Filled);
	}
}
