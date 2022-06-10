using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Core.CustomLeaderboard.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.CustomLeaderboard.Components;

public partial class Leaderboard
{
	[Parameter]
	[EditorRequired]
	public GetUploadSuccess UploadSuccess { get; set; } = null!;

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
