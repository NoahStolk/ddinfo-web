using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.App.Core.GameMemory;
using DevilDaggersInfo.Razor.CustomLeaderboard.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Components;

public partial class Leaderboard
{
	[Parameter]
	[EditorRequired]
	public GetCustomLeaderboard CustomLeaderboard { get; set; } = null!;

	[Inject]
	public NetworkService NetworkService { get; set; } = null!;

	[Inject]
	public GameMemoryReaderService ReaderService { get; set; } = null!;

	public async Task InjectReplay(int customEntryId)
	{
		byte[]? replayData = await NetworkService.GetReplay(customEntryId);
		if (replayData != null)
			ReaderService.WriteReplayToMemory(replayData);
	}
}
