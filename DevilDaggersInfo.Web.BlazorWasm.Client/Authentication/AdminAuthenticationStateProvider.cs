using Blazored.LocalStorage;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Authentication;

public class AdminAuthenticationStateProvider : AuthenticationStateProvider
{
	public const string LocalStorageAuthKey = "auth";

	private readonly ILocalStorageService _localStorageService;
	private readonly PublicApiHttpClient _httpClient;

	public AdminAuthenticationStateProvider(ILocalStorageService localStorageService, PublicApiHttpClient httpClient)
	{
		_localStorageService = localStorageService;
		_httpClient = httpClient;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		string token = await _localStorageService.GetItemAsStringAsync(LocalStorageAuthKey);
		if (string.IsNullOrEmpty(token))
			return await ClearTokenAndGetDefaultState();

		HttpResponseMessage httpResponseMessage = await _httpClient.Authenticate(new AuthenticationRequest { Jwt = token });

		if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
			return await ClearTokenAndGetDefaultState();

		AuthenticationResponse? authenticationResponse = await httpResponseMessage.Content.ReadFromJsonAsync<AuthenticationResponse>() ?? throw new($"Could not deserialize {nameof(AuthenticationResponse)}.");
		if (authenticationResponse == null || string.IsNullOrEmpty(authenticationResponse.Name))
			return await ClearTokenAndGetDefaultState();

		ClaimsIdentity claimsIdentity = ClaimsIdentityUtils.CreateClaimsIdentity(
			id: authenticationResponse.Id,
			name: authenticationResponse.Name,
			roles: authenticationResponse.RoleNames,
			playerId: authenticationResponse.PlayerId);
		ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

		return new AuthenticationState(claimsPrincipal);

		async Task<AuthenticationState> ClearTokenAndGetDefaultState()
		{
			if (await _localStorageService.ContainKeyAsync(LocalStorageAuthKey))
				await _localStorageService.RemoveItemAsync(LocalStorageAuthKey);

			return new(new ClaimsPrincipal(new ClaimsIdentity()));
		}
	}

	public async Task SetTokenAsync(string? token)
	{
		if (token == null)
		{
			if (await _localStorageService.ContainKeyAsync(LocalStorageAuthKey))
				await _localStorageService.RemoveItemAsync(LocalStorageAuthKey);
		}
		else
		{
			await _localStorageService.SetItemAsStringAsync(LocalStorageAuthKey, token);
		}

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}
}
