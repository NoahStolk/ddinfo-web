using DevilDaggersInfo.App.Ui.Base.StateManagement;
using DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.Actions;
using Warp.NET.Ui;
using Warp.NET.Ui.Components;

namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena.SettingsWrappers;

public class LineToolSettingsWrapper : AbstractComponent, ISettingsWrapper
{
	public LineToolSettingsWrapper(IBounds bounds)
		: base(bounds)
	{
		int y = 0;
		ISettingsWrapper wrapper = this;
		wrapper.AddSlider("Width", ref y, f => StateManager.Dispatch(new SetArenaLineWidth(f)), 0.1f, 10, 0.1f, StateManager.ArenaLineState.Width, "0.0");
	}
}
