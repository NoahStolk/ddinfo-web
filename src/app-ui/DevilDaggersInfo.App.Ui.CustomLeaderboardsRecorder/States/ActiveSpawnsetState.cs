namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;

public record ActiveSpawnsetState(string? Name)
{
	public static ActiveSpawnsetState GetDefault()
	{
		return new((string?)null);
	}
}
