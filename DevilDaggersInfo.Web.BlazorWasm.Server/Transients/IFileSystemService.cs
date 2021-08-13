using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;
using System;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Transients
{
	public interface IFileSystemService
	{
		string Root { get; }

		string[] TryGetFiles(DataSubDirectory subDirectory);

		string GetLeaderboardHistoryPathFromDate(DateTime dateTime);

		string GetPath(DataSubDirectory subDirectory);

		string GetRelevantDisplayPath(string path);
	}
}
