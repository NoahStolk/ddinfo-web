namespace DevilDaggersInfo.App.Ui.SurvivalEditor.Components.SpawnsetArena.SettingsWrappers;

public class LineToolSettingsWrapper : AbstractComponent, ISettingsWrapper
{
	public LineToolSettingsWrapper(IBounds bounds)
		: base(bounds)
	{
		int y = 0;
		ISettingsWrapper wrapper = this;
		wrapper.AddSlider("Thickness", ref y, f => StateManager.Dispatch(new SetArenaLineThickness(f)), 0, 10, 0.1f, StateManager.ArenaLineState.Thickness, "0.0");
	}
}
