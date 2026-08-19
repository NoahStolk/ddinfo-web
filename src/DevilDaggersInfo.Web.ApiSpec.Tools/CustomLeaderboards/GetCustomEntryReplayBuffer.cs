namespace DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;

public sealed record GetCustomEntryReplayBuffer
{
	public required byte[] Data { get; init; }
}
