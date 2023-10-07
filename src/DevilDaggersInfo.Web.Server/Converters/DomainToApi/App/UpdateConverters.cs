using DevilDaggersInfo.Web.ApiSpec.Tools.Updates;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;

public static class UpdateConverters
{
	public static GetLatestVersion ToAppApi(this ToolDistribution distribution) => new()
	{
		VersionNumber = distribution.VersionNumber,
		FileSize = distribution.FileSize,
	};
}
