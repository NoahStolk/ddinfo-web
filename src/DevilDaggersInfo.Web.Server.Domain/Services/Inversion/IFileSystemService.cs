using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;

namespace DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

public interface IFileSystemService
{
	string[] TryGetFiles(DataSubDirectory subDirectory);

	string GetLeaderboardHistoryPathFromDate(DateTime dateTime);

	string GetPath(DataSubDirectory subDirectory);

	Task<string?> GetModArchiveCacheDataJsonAsync(string modName);
}
