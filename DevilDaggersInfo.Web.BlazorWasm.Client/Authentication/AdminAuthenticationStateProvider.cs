using Blazored.LocalStorage;
using DevilDaggersInfo.Web.BlazorWasm.Client.HttpClients;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Authentication;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
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
			return DefaultState();

		HttpResponseMessage httpResponseMessage = await _httpClient.Authenticate(new AuthenticationRequest { Jwt = token });

		if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
			return DefaultState();

		AuthenticationResponse? authenticationResponse = await httpResponseMessage.Content.ReadFromJsonAsync<AuthenticationResponse>() ?? throw new($"Could not deserialize {nameof(AuthenticationResponse)}.");
		if (authenticationResponse == null || string.IsNullOrEmpty(authenticationResponse.Name))
			return DefaultState();

		Claim claimName = new(ClaimTypes.NameIdentifier, authenticationResponse.Name);
		Claim claimRole = new(ClaimTypes.Role, string.Join(',', authenticationResponse.RoleNames));
		ClaimsIdentity claimsIdentity = new(new[] { claimName, claimRole }, "serverAuth");
		ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

		return new AuthenticationState(claimsPrincipal);

		static AuthenticationState DefaultState() => new(new ClaimsPrincipal(new ClaimsIdentity()));
	}

	public async Task SetTokenAsync(string? token)
	{
		if (token == null)
			await _localStorageService.RemoveItemAsync(LocalStorageAuthKey);
		else
			await _localStorageService.SetItemAsStringAsync(LocalStorageAuthKey, token);

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}
}
