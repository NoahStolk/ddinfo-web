namespace DevilDaggersInfo.App.Ui.Base.StateManagement.SurvivalEditor.States;

public record ArenaLineState(float Width)
{
	public static ArenaLineState GetDefault()
	{
		return new(0.1f);
	}
}
