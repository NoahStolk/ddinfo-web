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
		if (DevilDaggersInfoServerConstants.FileMessage == null)
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

		await DevilDaggersInfoServerConstants.FileMessage.TryEdit(builder.Build());
	}

	private static DirectoryStatistics GetDirectorySize(string folderPath)
	{
		DirectoryInfo di = new(folderPath);
		IEnumerable<FileInfo> allFiles = di.EnumerateFiles("*.*", SearchOption.AllDirectories);
		return new()
		{
			Size = allFiles.Sum(fi => fi.Length),
			FileCount = allFiles.Count(),
		};
	}

	// TODO: Use .NET 6 record struct.
	private struct DirectoryStatistics
	{
		public long Size { get; init; }
		public int FileCount { get; init; }
	}
}
