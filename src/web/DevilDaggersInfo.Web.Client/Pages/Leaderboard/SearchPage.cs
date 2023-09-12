using DevilDaggersInfo.Api.Main.Leaderboards;
using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Core.Common.Extensions;
using DevilDaggersInfo.Web.Client.Extensions;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace DevilDaggersInfo.Web.Client.Pages.Leaderboard;

public partial class SearchPage
{
	private string? _apiError;
	private bool _loading;

	[Parameter]
	[SupplyParameterFromQuery]
	public string? Username { get; set; }

	public List<GetEntry>? GetEntries { get; set; }

	public List<GetPlayerForLeaderboard>? Players { get; set; }

	protected override async Task OnInitializedAsync()
	{
		_loading = CanLoad(Username);
		Players = await Http.GetPlayersForLeaderboard();
		await FetchLeaderboard();
	}

	private async Task ChangeInputUsername(ChangeEventArgs e)
	{
		string? username = e.Value?.ToString();

		Username = username;
		NavigationManager.AddOrModifyQueryParameter(QueryParameters.Username, Username);

		await FetchLeaderboard();
	}

	private async Task FetchLeaderboard()
	{
		_apiError = null;
		if (!CanLoad(Username))
		{
			_loading = false;
			GetEntries = null;
			return;
		}

		try
		{
			_loading = true;
			GetEntries = (await Http.GetEntriesByName(Username)).OrderBy(e => e.Rank, true).ToList();
		}
		catch (Exception ex)
		{
			_apiError = ex.Message;
		}
		finally
		{
			_loading = false;
		}
	}

	private static bool CanLoad([NotNullWhen(true)] string? username) => username is { Length: >= 3 and <= 16 };

	private static class QueryParameters
	{
		public static string Username { get; } = nameof(Username);
	}
}
