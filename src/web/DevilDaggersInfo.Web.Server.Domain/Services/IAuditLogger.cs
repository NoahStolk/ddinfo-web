using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public interface IAuditLogger
{
	void LogAdd(Dictionary<string, string> dtoLog, ClaimsPrincipal claimsPrincipal, int id, List<FileSystemInformation>? fileSystemInformation = null, string endpointName = "");

	void LogDelete(Dictionary<string, string> entityLog, ClaimsPrincipal claimsPrincipal, int id, List<FileSystemInformation>? fileSystemInformation = null, string endpointName = "");

	void LogEdit(Dictionary<string, string> oldDtoLog, Dictionary<string, string> dtoLog, ClaimsPrincipal claimsPrincipal, int id, List<FileSystemInformation>? fileSystemInformation = null, string endpointName = "");

	void LogPlayerUpdates(string caller, List<(int PlayerId, string OldValue, string NewValue)> logs);

	void LogRoleAssign(string userName, string roleName);

	void LogRoleRevoke(string userName, string roleName);
}
