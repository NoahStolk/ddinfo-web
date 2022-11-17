namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record GetCustomEntryReplayBuffer
{
	public required byte[] Data { get; init; }
}
