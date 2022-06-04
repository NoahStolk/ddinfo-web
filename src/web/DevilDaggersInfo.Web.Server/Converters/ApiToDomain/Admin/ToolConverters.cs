using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using AdminApi = DevilDaggersInfo.Api.Admin.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Admin;

public static class ToolConverters
{
	public static ToolBuildType ToDomain(this AdminApi.ToolBuildType buildType) => buildType switch
	{
		AdminApi.ToolBuildType.WindowsWpf => ToolBuildType.WindowsWpf,
		AdminApi.ToolBuildType.WindowsConsole => ToolBuildType.WindowsConsole,
		_ => throw new InvalidEnumConversionException(buildType),
	};

	public static ToolPublishMethod ToDomain(this AdminApi.ToolPublishMethod publishMethod) => publishMethod switch
	{
		AdminApi.ToolPublishMethod.Default => ToolPublishMethod.Default,
		AdminApi.ToolPublishMethod.SelfContained => ToolPublishMethod.SelfContained,
		_ => throw new InvalidEnumConversionException(publishMethod),
	};
}
