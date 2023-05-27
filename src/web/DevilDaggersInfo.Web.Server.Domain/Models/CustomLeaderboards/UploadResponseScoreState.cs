namespace DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;

public readonly record struct UploadResponseScoreState<T>(T Value, T ValueDifference = default)
	where T : struct;
