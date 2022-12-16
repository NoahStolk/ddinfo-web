namespace DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;

[Obsolete("DDCL 1.8.3 will be removed.")]
public readonly record struct GetScoreState<T>(T Value, T ValueDifference = default)
	where T : struct;
