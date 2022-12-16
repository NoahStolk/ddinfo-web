namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

[Obsolete("DDCL 1.8.3 will be removed.")]
public record GetCustomEntryReplayBuffer
{
	public required byte[] Data { get; init; }
}
