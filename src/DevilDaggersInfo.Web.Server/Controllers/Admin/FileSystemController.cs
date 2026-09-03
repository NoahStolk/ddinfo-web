using DevilDaggersInfo.Web.ApiSpec.Admin.FileSystem;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Constants;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/file-system")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public sealed class FileSystemController(IFileSystemService fileSystemService) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetFileSystemInfo> GetFileSystemInfo()
	{
		List<GetFileSystemEntry> entries =
		[
			.. Enum.GetValues<DataSubDirectory>()
				.OrderBy(subDir => subDir.ToString())
				.Select(subDir =>
				{
					DirectoryStatistics statistics = GetDirectorySize(fileSystemService.GetPath(subDir));
					return new GetFileSystemEntry
					{
						Count = statistics.FileCount,
						Size = statistics.Size,
						Name = subDir.ToString(),
					};
				}),
		];

		return new GetFileSystemInfo
		{
			TotalSize = entries.Sum(entry => entry.Size),
			AllowedSizeByModArchiveProcessor = ModConstants.BinaryMaxHostingSpace,
			Entries = entries,
		};
	}

	private static DirectoryStatistics GetDirectorySize(string folderPath)
	{
		DirectoryInfo di = new(folderPath);
		List<FileInfo> allFiles = [.. di.EnumerateFiles("*.*", SearchOption.AllDirectories)];
		return new DirectoryStatistics(allFiles.Sum(fi => fi.Length), allFiles.Count);
	}

	private readonly record struct DirectoryStatistics(long Size, int FileCount);
}
