using DevilDaggersInfo.Web.BlazorWasm.Client.Authentication;
using DevilDaggersInfo.Web.BlazorWasm.Client.Components.Admin;
using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Players;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Pages.Profile;

public partial class EditProfilePage
{
	private AuthenticationState? _state;
	private int _playerId;
	private bool? _linked;

	private EditPlayerProfile _editPlayer = new();

	public ErrorState State { get; set; }
	public string? Message { get; set; }

	[Inject]
	public AdminAuthenticationStateProvider Auth { get; set; } = null!;

	[Inject]
	public NavigationManager NavigationManager { get; set; } = null!;

	[Inject]
	public PublicApiHttpClient Http { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		_state = await Auth.GetAuthenticationStateAsync();
		if (_state.User?.Identity?.IsAuthenticated != true)
			NavigationManager.NavigateTo("/authentication/login");

		_linked = int.TryParse(_state.User?.Claims?.FirstOrDefault(c => c.Type == "playerId")?.Value, out _playerId);

		try
		{
			GetPlayerProfile getPlayer = await Http.GetProfileByPlayerId(_playerId);
			_editPlayer = new()
			{
				CountryCode = getPlayer.CountryCode,
				Dpi = getPlayer.Dpi,
				Fov = getPlayer.Fov,
				Gamma = getPlayer.Gamma,
				HasFlashHandEnabled = getPlayer.HasFlashHandEnabled,
				HideDonations = getPlayer.HideDonations,
				HidePastUsernames = getPlayer.HidePastUsernames,
				HideSettings = getPlayer.HideSettings,
				InGameSens = getPlayer.InGameSens,
				IsRightHanded = getPlayer.IsRightHanded,
				UsesLegacyAudio = getPlayer.UsesLegacyAudio,
				UsesHrtf = getPlayer.UsesHrtf,
				UsesInvertY = getPlayer.UsesInvertY,
				VerticalSync = getPlayer.VerticalSync,
			};
			State = ErrorState.None;
		}
		catch (HttpRequestException ex)
		{
			if (ex.StatusCode.HasValue)
				Message = $"Error {(int)ex.StatusCode}: {ex.StatusCode}";
			else
				Message = "An error occurred while sending the request.";

			State = ErrorState.FatalError;
		}
		catch (AccessTokenNotAvailableException exception)
		{
			exception.Redirect();
		}
	}

	private async Task OnValidSubmit()
	{
		if (_editPlayer == null || _playerId == 0 || !_linked.HasValue || !_linked.Value)
			return;

		try
		{
			HttpResponseMessage hrm = await Http.UpdateProfileByPlayerId(_playerId, _editPlayer);
			if (hrm.StatusCode == HttpStatusCode.OK)
			{
				Message = "Successfully updated profile.";
			}
			else
			{
				Message = await hrm.Content.ReadAsStringAsync();
				State = hrm.StatusCode.GetErrorState();
			}
		}
		catch (AccessTokenNotAvailableException exception)
		{
			exception.Redirect();
		}
	}

	private void Dismiss()
	{
		State = ErrorState.None;
		Message = null;
		StateHasChanged();
	}
}
