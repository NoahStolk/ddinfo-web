using DevilDaggersInfo.Core.Versioning;

namespace DevilDaggersInfo.Web.Server.Domain.Constants;

public static class FeatureConstants
{
	public static AppVersion OldDdclGraphs { get; } = new(1, 2, 0);

	public static AppVersion OldDdclHomingEaten { get; } = new(0, 14, 5);

	public static AppVersion OldDdclV3_1 { get; } = new(0, 14, 1);
}
