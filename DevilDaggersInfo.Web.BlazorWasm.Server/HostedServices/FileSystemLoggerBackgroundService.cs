using DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices.DdInfoDiscordBot;
using DSharpPlus.Entities;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.HostedServices;

public class FileSystemLoggerBackgroundService : AbstractBackgroundService
{
	private readonly IFileSystemService _fileSystemService;

	public FileSystemLoggerBackgroundService(IFileSystemService fileSystemService, BackgroundServiceMonitor backgroundServiceMonitor, ILogger<FileSystemLoggerBackgroundService> logger)
		: base(backgroundServiceMonitor, logger)
	{
		_fileSystemService = fileSystemService;
	}

	protected override TimeSpan Interval => TimeSpan.FromMinutes(5);

	protected override async Task ExecuteTaskAsync(CancellationToken stoppingToken)
	{
		if (DiscordServerConstants.FileMessage == null)
			return;

		DiscordEmbedBuilder builder = new()
		{
			Title = $"File {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}",
			Color = DiscordColor.White,
		};

		DataSubDirectory[] dataSubDirectories = Enum.GetValues<DataSubDirectory>();
		for (int i = 0; i < dataSubDirectories.Length; i++)
		{
			DataSubDirectory dataSubDirectory = dataSubDirectories[i];
			DirectoryStatistics statistics = GetDirectorySize(_fileSystemService.GetPath(dataSubDirectory));
			builder.AddFieldObject(dataSubDirectory.ToString(), $"`{statistics.Size:n0}` bytes\n`{statistics.FileCount}` files");
		}

		await DiscordServerConstants.FileMessage.TryEdit(builder.Build());
	}

	private static DirectoryStatistics GetDirectorySize(string folderPath)
	{
		DirectoryInfo di = new(folderPath);
		IEnumerable<FileInfo> allFiles = di.EnumerateFiles("*.*", SearchOption.AllDirectories);
		return new(allFiles.Sum(fi => fi.Length), allFiles.Count());
	}

	private readonly record struct DirectoryStatistics(long Size, int FileCount);
}
