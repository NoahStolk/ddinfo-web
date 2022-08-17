using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Web.Client.Authentication;
using DevilDaggersInfo.Web.Client.Enums;
using DevilDaggersInfo.Web.Client.Extensions;
using DevilDaggersInfo.Web.Client.HttpClients;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.Client.Pages.Profile;

public partial class EditProfilePage
{
	private AuthenticationState? _state;
	private int _playerId;
	private bool? _linked;

	private readonly EditPlayerProfile _editPlayer = new();

	public ErrorState State { get; set; }
	public string? Message { get; set; }

	[Inject]
	public AdminAuthenticationStateProvider Auth { get; set; } = null!;

	[Inject]
	public NavigationManager NavigationManager { get; set; } = null!;

	[Inject]
	public MainApiHttpClient Http { get; set; } = null!;

	protected override async Task OnInitializedAsync()
	{
		_state = await Auth.GetAuthenticationStateAsync();
		if (_state.User.Identity?.IsAuthenticated != true)
			NavigationManager.NavigateTo("/authentication/login");

		_linked = int.TryParse(_state.User.Claims.FirstOrDefault(c => c.Type == "playerId")?.Value, out _playerId);

		try
		{
			GetPlayerProfile getPlayer = await Http.GetProfileByPlayerId(_playerId);
			_editPlayer.CountryCode = getPlayer.CountryCode;
			_editPlayer.Dpi = getPlayer.Dpi;
			_editPlayer.Fov = getPlayer.Fov;
			_editPlayer.Gamma = getPlayer.Gamma;
			_editPlayer.HasFlashHandEnabled = getPlayer.HasFlashHandEnabled;
			_editPlayer.HideDonations = getPlayer.HideDonations;
			_editPlayer.HidePastUsernames = getPlayer.HidePastUsernames;
			_editPlayer.HideSettings = getPlayer.HideSettings;
			_editPlayer.InGameSens = getPlayer.InGameSens;
			_editPlayer.IsRightHanded = getPlayer.IsRightHanded;
			_editPlayer.UsesLegacyAudio = getPlayer.UsesLegacyAudio;
			_editPlayer.UsesHrtf = getPlayer.UsesHrtf;
			_editPlayer.UsesInvertY = getPlayer.UsesInvertY;
			_editPlayer.VerticalSync = getPlayer.VerticalSync;
			State = ErrorState.None;
		}
		catch (HttpRequestException ex)
		{
			Message = ex.StatusCode.HasValue ? $"Error {(int)ex.StatusCode}: {ex.StatusCode}" : "An error occurred while sending the request.";

			State = ErrorState.FatalError;
		}
		catch (AccessTokenNotAvailableException exception)
		{
			exception.Redirect();
		}
	}

	private async Task OnValidSubmit()
	{
		if (_playerId == 0 || !_linked.HasValue || !_linked.Value)
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
