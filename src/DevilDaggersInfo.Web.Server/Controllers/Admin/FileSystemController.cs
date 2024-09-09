using DevilDaggersInfo.Web.ApiSpec.Admin.FileSystem;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
				return new GetFileSystemEntry
				{
					Count = statistics.FileCount,
					Size = statistics.Size,
					Name = dsd.ToString(),
				};
			})
			.ToList();
	}

	private static DirectoryStatistics GetDirectorySize(string folderPath)
	{
		DirectoryInfo di = new(folderPath);
		List<FileInfo> allFiles = di.EnumerateFiles("*.*", SearchOption.AllDirectories).ToList();
		return new DirectoryStatistics(allFiles.Sum(fi => fi.Length), allFiles.Count);
	}

	private readonly record struct DirectoryStatistics(long Size, int FileCount);
}
