using DevilDaggersInfo.Web.Shared.Dto.Admin.FileSystem;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/file-system")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class FileSystemController : ControllerBase
{
	private readonly IFileSystemService _fileSystemService;

	public FileSystemController(IFileSystemService fileSystemService)
	{
		_fileSystemService = fileSystemService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetFileSystemEntry>> GetFileSystemInfo()
	{
		return Enum.GetValues<DataSubDirectory>()
			.OrderBy(dsd => dsd.ToString())
			.Select(dsd =>
			{
				DirectoryStatistics statistics = GetDirectorySize(_fileSystemService.GetPath(dsd));
				return new GetFileSystemEntry(dsd.ToString(), statistics.FileCount, statistics.Size);
			})
			.ToList();
	}

	private static DirectoryStatistics GetDirectorySize(string folderPath)
	{
		DirectoryInfo di = new(folderPath);
		IEnumerable<FileInfo> allFiles = di.EnumerateFiles("*.*", SearchOption.AllDirectories);
		return new(allFiles.Sum(fi => fi.Length), allFiles.Count());
	}

	private readonly record struct DirectoryStatistics(long Size, int FileCount);
}
