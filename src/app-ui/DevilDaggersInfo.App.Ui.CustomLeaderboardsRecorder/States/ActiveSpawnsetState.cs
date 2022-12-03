namespace DevilDaggersInfo.App.Ui.CustomLeaderboardsRecorder.States;

public record ActiveSpawnsetState(string? Name, byte[]? FileContents, byte[]? FileHash)
{
	public static ActiveSpawnsetState GetDefault()
	{
		return new(null, null, null);
	}
}
