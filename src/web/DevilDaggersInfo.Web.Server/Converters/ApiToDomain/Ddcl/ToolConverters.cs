using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DdclApi = DevilDaggersInfo.Api.Ddcl.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Ddcl;

public static class ToolConverters
{
	public static ToolBuildType ToDomain(this DdclApi.ToolBuildType buildType) => buildType switch
	{
		DdclApi.ToolBuildType.WindowsWpf => ToolBuildType.WindowsWpf,
		DdclApi.ToolBuildType.WindowsConsole => ToolBuildType.WindowsConsole,
		_ => throw new InvalidEnumConversionException(buildType),
	};

	public static ToolPublishMethod ToDomain(this DdclApi.ToolPublishMethod publishMethod) => publishMethod switch
	{
		DdclApi.ToolPublishMethod.Default => ToolPublishMethod.Default,
		DdclApi.ToolPublishMethod.SelfContained => ToolPublishMethod.SelfContained,
		_ => throw new InvalidEnumConversionException(publishMethod),
	};
}
