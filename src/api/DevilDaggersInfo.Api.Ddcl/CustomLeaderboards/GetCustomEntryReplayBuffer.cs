namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

public record GetCustomEntryReplayBuffer
{
	public byte[] Data { get; init; } = null!;
}
