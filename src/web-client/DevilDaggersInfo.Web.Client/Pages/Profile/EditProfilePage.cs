using DevilDaggersInfo.Api.Main.Players;
using DevilDaggersInfo.Web.Client.Authentication;
using DevilDaggersInfo.Web.Client.Enums;
using DevilDaggersInfo.Web.Client.Extensions;
using DevilDaggersInfo.Web.Client.HttpClients;
using DevilDaggersInfo.Web.Client.StateObjects.Profile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.Client.Pages.Profile;

public partial class EditProfilePage
{
	private AuthenticationState? _state;
	private int _playerId;
	private bool? _linked;

	private readonly EditPlayerProfileState _editPlayerState = new();

	public ErrorState State { get; set; }
	public string? Message { get; set; }

	[Inject]
	public required AdminAuthenticationStateProvider Auth { get; set; }

	[Inject]
	public required NavigationManager NavigationManager { get; set; }

	[Inject]
	public required MainApiHttpClient Http { get; set; }

	protected override async Task OnInitializedAsync()
	{
		_state = await Auth.GetAuthenticationStateAsync();
		if (_state.User.Identity?.IsAuthenticated != true)
			NavigationManager.NavigateTo("/authentication/login");

		_linked = int.TryParse(_state.User.Claims.FirstOrDefault(c => c.Type == "playerId")?.Value, out _playerId);

		try
		{
			GetPlayerProfile getPlayer = await Http.GetProfileByPlayerId(_playerId);
			_editPlayerState.CountryCode = getPlayer.CountryCode;
			_editPlayerState.Dpi = getPlayer.Dpi;
			_editPlayerState.Fov = getPlayer.Fov;
			_editPlayerState.Gamma = getPlayer.Gamma;
			_editPlayerState.HasFlashHandEnabled = getPlayer.HasFlashHandEnabled;
			_editPlayerState.HideDonations = getPlayer.HideDonations;
			_editPlayerState.HidePastUsernames = getPlayer.HidePastUsernames;
			_editPlayerState.HideSettings = getPlayer.HideSettings;
			_editPlayerState.InGameSens = getPlayer.InGameSens;
			_editPlayerState.IsRightHanded = getPlayer.IsRightHanded;
			_editPlayerState.UsesLegacyAudio = getPlayer.UsesLegacyAudio;
			_editPlayerState.UsesHrtf = getPlayer.UsesHrtf;
			_editPlayerState.UsesInvertY = getPlayer.UsesInvertY;
			_editPlayerState.VerticalSync = getPlayer.VerticalSync;
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
			HttpResponseMessage hrm = await Http.UpdateProfileByPlayerId(_playerId, _editPlayerState.ToModel());
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
