using Blazored.LocalStorage;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Extensions;
using Microsoft.AspNetCore.Components.Authorization;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Authentication;

public class AdminAuthenticationStateProvider : AuthenticationStateProvider
{
	private const string _localStorageAuthKey = "auth";

	private readonly ILocalStorageService _localStorageService;

	public AdminAuthenticationStateProvider(ILocalStorageService localStorageService)
	{
		_localStorageService = localStorageService;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		string token = await _localStorageService.GetItemAsStringAsync(_localStorageAuthKey);
		return await Task.FromResult(new AuthenticationState(token.CreateClaimsPrincipalFromJwtTokenString()));
	}

	public async Task SetTokenAsync(string? token)
	{
		if (token == null)
			await _localStorageService.RemoveItemAsync(_localStorageAuthKey);
		else
			await _localStorageService.SetItemAsStringAsync(_localStorageAuthKey, token);

		NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
	}
}
