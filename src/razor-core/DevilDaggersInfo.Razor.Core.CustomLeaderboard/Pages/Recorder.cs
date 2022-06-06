using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Core.CustomLeaderboard.Enums;
using DevilDaggersInfo.Core.CustomLeaderboard.Memory;
using DevilDaggersInfo.Core.CustomLeaderboard.Services;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Razor.Core.CustomLeaderboard.Pages;

public partial class Recorder
{
	private long? _marker;
	private bool _isRecording = true;
	private MainBlock _finalRecordedMainBlock;
	private GetUploadSuccess? _uploadSuccess;

	[Inject]
	public IClientConfiguration ClientConfiguration { get; set; } = null!;

	[Inject]
	public NetworkService NetworkService { get; set; } = null!;

	[Inject]
	public ReaderService ReaderService { get; set; } = null!;

	[Inject]
	public UploadService UploadService { get; set; } = null!;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		while (true)
		{
			await Task.Delay(50);
			await Record();

			StateHasChanged();
		}
	}

	private async Task Record()
	{
		if (!_marker.HasValue)
		{
			_marker = await NetworkService.GetMarker(ClientConfiguration.GetOperatingSystem());
			if (!_marker.HasValue)
				return;
		}

		if (!ReaderService.FindWindow())
			return;

		if (!ReaderService.Initialize(_marker.Value))
			return;

		ReaderService.Scan();

		if (!_isRecording)
		{
			if (ReaderService.MainBlock.Time == ReaderService.MainBlockPrevious.Time || ReaderService.MainBlock.Status == (int)GameStatus.LocalReplay)
				return;

			_isRecording = true;
			_uploadSuccess = null;
		}

		bool justDied = !ReaderService.MainBlock.IsPlayerAlive && ReaderService.MainBlockPrevious.IsPlayerAlive;
		bool uploadRun = justDied && (ReaderService.MainBlock.GameMode == 0 || ReaderService.MainBlock.TimeAttackOrRaceFinished);
		if (!uploadRun)
			return;

		if (!ReaderService.MainBlock.StatsLoaded)
		{
			await Task.Delay(TimeSpan.FromSeconds(0.5));
			return;
		}

		if (!ReaderService.IsReplayValid())
		{
			await Task.Delay(TimeSpan.FromSeconds(0.5));
			return;
		}

		_isRecording = false;

		string? errorMessage = ReaderService.ValidateRunLocally();
		if (errorMessage == null)
		{
			GetUploadSuccess? uploadSuccess = await UploadService.UploadRun();
			if (uploadSuccess != null)
			{
				_uploadSuccess = uploadSuccess;
				_finalRecordedMainBlock = ReaderService.MainBlock;
			}
			else
			{
				await Task.Delay(TimeSpan.FromSeconds(0.5));
			}
		}
		else
		{
			await Task.Delay(TimeSpan.FromSeconds(0.5));
		}
	}
}
