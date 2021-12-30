using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Admin;

public partial class AdminAdd<TModel>
{
	[Parameter, EditorRequired] public string Name { get; set; } = null!;
	[Parameter, EditorRequired] public string OverviewUrl { get; set; } = null!;
	[Parameter, EditorRequired] public Func<TModel, Task<HttpResponseMessage>> ApiCall { get; set; } = null!;
	[Parameter, EditorRequired] public TModel Model { get; set; } = default!;
	[Parameter, EditorRequired] public RenderFragment ChildContent { get; set; } = null!;

	public ErrorState State { get; set; }
	public string? ErrorMessage { get; set; }

	private async Task OnValidSubmit()
	{
		try
		{
			HttpResponseMessage hrm = await ApiCall.Invoke(Model);

			if (hrm.StatusCode == HttpStatusCode.OK)
				NavigationManager.NavigateTo(OverviewUrl);
			else
				ErrorMessage = await hrm.Content.ReadAsStringAsync();

			State = hrm.StatusCode.GetErrorState();
		}
		catch (AccessTokenNotAvailableException exception)
		{
			exception.Redirect();
		}
	}

	private void Dismiss()
	{
		State = ErrorState.None;
		StateHasChanged();
	}
}
