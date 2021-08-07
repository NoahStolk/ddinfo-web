using DevilDaggersWebsite.BlazorWasm.Server.Enumerators;
using System;

namespace DevilDaggersWebsite.BlazorWasm.Server.Transients
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
