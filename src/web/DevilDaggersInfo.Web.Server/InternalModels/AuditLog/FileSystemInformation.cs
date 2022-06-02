using DevilDaggersInfo.Web.Server.Enums;

namespace DevilDaggersInfo.Web.Server.InternalModels.AuditLog;

public record FileSystemInformation(string Message, FileSystemInformationType Type);
