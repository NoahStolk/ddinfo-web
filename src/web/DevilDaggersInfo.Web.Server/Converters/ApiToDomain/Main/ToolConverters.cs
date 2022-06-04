using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using MainApi = DevilDaggersInfo.Api.Main.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Main;

public static class ToolConverters
{
	public static ToolBuildType ToDomain(this MainApi.ToolBuildType buildType) => buildType switch
	{
		MainApi.ToolBuildType.WindowsWpf => ToolBuildType.WindowsWpf,
		MainApi.ToolBuildType.WindowsConsole => ToolBuildType.WindowsConsole,
		_ => throw new InvalidEnumConversionException(buildType),
	};

	public static ToolPublishMethod ToDomain(this MainApi.ToolPublishMethod publishMethod) => publishMethod switch
	{
		MainApi.ToolPublishMethod.Default => ToolPublishMethod.Default,
		MainApi.ToolPublishMethod.SelfContained => ToolPublishMethod.SelfContained,
		_ => throw new InvalidEnumConversionException(publishMethod),
	};
}
