using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Core.CustomLeaderboard.Models;
using DevilDaggersInfo.Core.CustomLeaderboard.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Components;

public partial class Leaderboard
{
	[Parameter]
	[EditorRequired]
	public ResponseWrapper<GetCustomLeaderboard> CustomLeaderboard { get; set; } = null!;

	[Inject]
	public NetworkService NetworkService { get; set; } = null!;

	[Inject]
	public ReaderService ReaderService { get; set; } = null!;

	public async Task InjectReplay(int customEntryId)
	{
		byte[]? replayData = await NetworkService.GetReplay(customEntryId);
		if (replayData != null)
			ReaderService.WriteReplayToMemory(replayData);
	}
}
