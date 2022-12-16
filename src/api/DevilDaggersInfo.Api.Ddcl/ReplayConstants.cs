namespace DevilDaggersInfo.Api.Ddcl;

[Obsolete("DDCL 1.8.3 will be removed.")]
public static class ReplayConstants
{
	public const int MaxFileSize = 30 * 1024 * 1024;

	public const string MaxFileSizeErrorMessage = "Replay buffer cannot be larger than 31,457,280 bytes.";
}
