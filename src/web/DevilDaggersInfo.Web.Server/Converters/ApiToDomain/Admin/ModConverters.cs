using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using AdminApi = DevilDaggersInfo.Api.Admin.Mods;

namespace DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Admin;

public static class ModConverters
{
	// TODO: Remove cast.
	public static ModTypes ToDomain(this AdminApi.ModTypes modTypes) => (ModTypes)modTypes;
}
