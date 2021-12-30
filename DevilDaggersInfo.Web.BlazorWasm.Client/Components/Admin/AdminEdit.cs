using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Admin;

public partial class AdminEdit<TModel>
{
	[Parameter, EditorRequired] public string Name { get; set; } = null!;
	[Parameter, EditorRequired] public string OverviewUrl { get; set; } = null!;
	[Parameter, EditorRequired] public Func<int, TModel, Task<HttpResponseMessage>> ApiCall { get; set; } = null!;
	[Parameter, EditorRequired] public TModel? Model { get; set; }
	[Parameter, EditorRequired] public int Id { get; set; }
	[Parameter, EditorRequired] public RenderFragment ChildContent { get; set; } = null!;

	public ErrorState State { get; set; }
	public string? ErrorMessage { get; set; }

	private async Task OnValidSubmit()
	{
		if (Model == null)
			return;

		try
		{
			HttpResponseMessage hrm = await ApiCall.Invoke(Id, Model);

			if (hrm.StatusCode == HttpStatusCode.OK)
			{
				NavigationManager.NavigateTo(OverviewUrl);
			}
			else
			{
				ErrorMessage = await hrm.Content.ReadAsStringAsync();
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
		StateHasChanged();
	}
}
