using Blazorise.Charts;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomEntries;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.CustomLeaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Leaderboards;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;
using Microsoft.AspNetCore.Components;
using System;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Leaderboard;

public partial class PlayerPage
{
	private LineChart<double>? _progressionChart;
	private LineChart<double>? _activityChart;

	[Parameter, EditorRequired] public int Id { get; set; }

	[Inject]
	public PublicApiHttpClient Http { get; set; } = null!;

	[Inject]
	public NavigationManager NavigationManager { get; set; } = null!;

	public GetEntry? GetEntry { get; set; }
	public GetPlayer? GetPlayer { get; set; }
	public GetPlayerHistory? GetPlayerHistory { get; set; }
	public List<GetCustomLeaderboardStatisticsForPlayer>? GetCustomLeaderboardStatistics { get; set; }
	public List<GetSpawnsetName>? GetSpawnsetNames { get; set; }
	public List<GetModName>? GetModNames { get; set; }
	public GetNumberOfCustomLeaderboards? GetNumberOfCustomLeaderboards { get; set; }

	public bool PlayerNotFound { get; set; }

	protected override async Task OnInitializedAsync()
	{
		GetEntry = await Http.GetEntryById(Id);

		try
		{
			GetPlayer = await Http.GetPlayerById(Id);
		}
		catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
		{
			PlayerNotFound = true;
		}

		GetPlayerHistory = await Http.GetPlayerHistoryById(Id);
		GetCustomLeaderboardStatistics = await Http.GetCustomLeaderboardStatisticsByPlayerId(Id);
		GetSpawnsetNames = await Http.GetSpawnsetsByAuthorId(Id);
		GetModNames = await Http.GetModsByAuthorId(Id);
		GetNumberOfCustomLeaderboards = await Http.GetNumberOfCustomLeaderboards();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender ||
			GetPlayerHistory == null ||
			_progressionChart == null ||
			_activityChart == null)
			return;

		await _progressionChart.AddLabelsDatasetsAndUpdate(GetPlayerHistory.History.Select(eh => eh.DateTime.ToString("yyyy-MM-dd")).ToArray(), new LineChartDataset<double>
		{
			Label = "Score",
			Data = GetPlayerHistory.History.Select(eh => eh.Time).ToList(),
			BackgroundColor = "#f00",
			Fill = false,
			PointRadius = 0,
			ShowLine = true,
			BorderColor = "#f00",
			SteppedLine = true,
		});

		await _activityChart.AddLabelsDatasetsAndUpdate(GetPlayerHistory.Activity.Select(pa => pa.DateTime.ToString("yyyy-MM-dd")).ToArray(), new LineChartDataset<double>
		{
			Label = "Deaths",
			Data = GetPlayerHistory.Activity.Select(pa => pa.DeathsIncrement).ToList(),
			BackgroundColor = "#f00",
			Fill = false,
			PointRadius = 0,
			ShowLine = true,
			BorderColor = "#f00",
			SteppedLine = true,
		});
	}
}
