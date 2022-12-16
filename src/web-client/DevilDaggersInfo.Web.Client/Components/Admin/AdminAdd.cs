using DevilDaggersInfo.Web.Client.Enums;
using DevilDaggersInfo.Web.Client.Extensions;
using DevilDaggersInfo.Web.Client.StateObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net;

namespace DevilDaggersInfo.Web.Client.Components.Admin;

public partial class AdminAdd<TStateObject, TModel>
	where TStateObject : IStateObject<TModel>
{
	private bool _submitting;

	[Parameter]
	[EditorRequired]
	public required string Name { get; set; }

	[Parameter]
	[EditorRequired]
	public required string OverviewUrl { get; set; }

	[Parameter]
	[EditorRequired]
	public required Func<TModel, Task<HttpResponseMessage>> ApiCall { get; set; }

	[Parameter]
	[EditorRequired]
	public required TStateObject StateObject { get; set; }

	[Parameter]
	public required RenderFragment ChildContent { get; set; }

	[Parameter]
	[EditorRequired]
	public required Func<AdminAdd<TStateObject, TModel>, Task> OnPopulate { get; set; }

	public ErrorState State { get; set; }
	public string? ErrorMessage { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await OnPopulate(this);
	}

	private async Task OnValidSubmit()
	{
		_submitting = true;

		try
		{
			HttpResponseMessage hrm = await ApiCall.Invoke(StateObject.ToModel());

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

		_submitting = false;
	}

	private void Dismiss()
	{
		State = ErrorState.None;
		StateHasChanged();
	}
}
