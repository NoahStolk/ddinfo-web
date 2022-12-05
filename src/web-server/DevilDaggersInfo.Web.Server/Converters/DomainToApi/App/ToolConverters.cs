using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using AppApi = DevilDaggersInfo.Api.App.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;

public static class ToolConverters
{
	public static AppApi.GetToolDistribution ToAppApi(this ToolDistribution distribution) => new()
	{
		BuildType = distribution.BuildType,
		PublishMethod = distribution.PublishMethod,
		VersionNumber = distribution.VersionNumber,
		FileSize = distribution.FileSize,
	};
}
