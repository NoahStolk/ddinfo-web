namespace DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;

public readonly record struct UploadResponseScoreState<T>(T Value, T ValueDifference = default)
	where T : struct;
