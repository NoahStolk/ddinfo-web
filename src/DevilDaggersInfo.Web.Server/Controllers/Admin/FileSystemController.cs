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
public sealed class FileSystemController(IFileSystem fileSystem) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<GetFileSystemInfo>> GetFileSystemInfo()
	{
		List<GetFileSystemEntry> entries = [];
		foreach (DataSubDirectory subDir in Enum.GetValues<DataSubDirectory>().OrderBy(subDir => subDir.ToString()))
		{
			IReadOnlyList<FileEntry> files = await fileSystem.ListAsync(subDir);
			entries.Add(new GetFileSystemEntry
			{
				Count = files.Count,
				Size = files.Sum(f => f.Size),
				Name = subDir.ToString(),
			});
		}

		return new GetFileSystemInfo
		{
			TotalSize = entries.Sum(entry => entry.Size),
			AllowedSizeByModArchiveProcessor = ModConstants.BinaryMaxHostingSpace,
			Entries = entries,
		};
	}
}
