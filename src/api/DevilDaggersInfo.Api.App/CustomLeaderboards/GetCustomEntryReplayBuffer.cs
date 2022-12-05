namespace DevilDaggersInfo.Api.App.CustomLeaderboards;

public record GetCustomEntryReplayBuffer
{
	public required byte[] Data { get; init; }
}
