using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Ddiam;

public static class AppConverters
{
	public static OperatingSystemType ToDomain(this Api.Ddiam.OperatingSystemType os) => os switch
	{
		Api.Ddiam.OperatingSystemType.Windows => OperatingSystemType.Windows,
		Api.Ddiam.OperatingSystemType.Windows7 => OperatingSystemType.Windows7,
		Api.Ddiam.OperatingSystemType.Linux => OperatingSystemType.Linux,
		_ => throw new InvalidEnumConversionException(os),
	};
}
