namespace DevilDaggersInfo.Web.ApiSpec.App.CustomLeaderboards;

public record GetCustomEntryReplayBuffer
{
	public required byte[] Data { get; init; }
}
