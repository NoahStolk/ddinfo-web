namespace DevilDaggersInfo.Web.BlazorWasm.Server.Singletons.AuditLog;

public class FileSystemInformation
{
	public FileSystemInformation(string message, FileSystemInformationType type)
	{
		Message = message;
		Type = type;
	}

	public string Message { get; }

	public FileSystemInformationType Type { get; }
}
