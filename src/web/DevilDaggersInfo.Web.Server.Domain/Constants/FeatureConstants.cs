using DevilDaggersInfo.Core.Versioning;

namespace DevilDaggersInfo.Web.Server.Domain.Constants;

public static class FeatureConstants
{
	public static AppVersion DdclGraphs { get; } = new(1, 2, 0);

	public static AppVersion DdclHomingEaten { get; } = new(0, 14, 5);

	public static AppVersion DdclV3_1 { get; } = new(0, 10, 4);
}
