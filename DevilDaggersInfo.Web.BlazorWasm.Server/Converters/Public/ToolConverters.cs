using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Tools;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;

public static class ToolConverters
{
	public static GetTool ToGetTool(this Tool tool, ToolStatisticEntity? toolStatistic, int fileSize) => new()
	{
		Changelog = tool.Changelog
			.Select(ce => ce.ToGetChangelogEntry())
			.ToList(),
		DisplayName = tool.DisplayName,
		DownloadCount = toolStatistic?.DownloadCount ?? 0,
		Name = tool.Name,
		FileSize = fileSize,
		SupportedOperatingSystems = new() { "Windows 64-bit" }, // TODO: Get this from database. Also, DDSE is actually supported on 32-bit, but DD itself isn't 32-bit anymore so probably not worth mentioning.
		VersionNumber = tool.VersionNumber,
		VersionNumberRequired = tool.VersionNumberRequired,
	};

	private static GetChangelogEntry ToGetChangelogEntry(this ChangelogEntry changelogEntry) => new()
	{
		Changes = changelogEntry.Changes
			.Select(c => c.ToGetChange())
			.ToList(),
		Date = changelogEntry.Date,
		VersionNumber = changelogEntry.VersionNumber,
	};

	private static GetChange ToGetChange(this Change change) => new()
	{
		Description = change.Description,
		SubChanges = change.SubChanges?
			.Select(c => c.ToGetChange())
			.ToList(),
	};
}
