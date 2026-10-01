using DevilDaggersInfo.Web.Server.Domain.Utils;

namespace DevilDaggersInfo.Web.Server.Domain.Test.Tests.ServerDomain;

internal sealed class HistoryUtilsTests
{
	private static readonly string[] _fileNames = ["202201010000.bin", "202201030000.bin", "202201020000.bin", "notes.txt"];

	[Test]
	[Arguments(2022, 1, 1, "202201010000.bin")]
	[Arguments(2022, 1, 2, "202201020000.bin")]
	[Arguments(2022, 6, 1, "202201030000.bin")]
	public async Task GetHistoryFileNameFromDate_ReturnsLatestFileAtOrBeforeDate(int year, int month, int day, string expected)
	{
		DateTime dateTime = new(year, month, day, 12, 0, 0, DateTimeKind.Utc);
		await Assert.That(HistoryUtils.GetHistoryFileNameFromDate(_fileNames, dateTime)).IsEqualTo(expected);
	}

	[Test]
	public async Task GetHistoryFileNameFromDate_ReturnsFirstFileWhenDateIsBeforeAllFiles()
	{
		DateTime dateTime = new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
		await Assert.That(HistoryUtils.GetHistoryFileNameFromDate(_fileNames, dateTime)).IsEqualTo("202201010000.bin");
	}
}
