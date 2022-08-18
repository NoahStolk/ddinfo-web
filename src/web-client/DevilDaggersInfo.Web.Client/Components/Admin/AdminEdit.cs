using DevilDaggersInfo.Web.Client.Enums;
using DevilDaggersInfo.Web.Client.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.Client.Components.Admin;

public partial class AdminEdit<TModel>
{
	private bool _submitting;

	[Parameter]
	[EditorRequired]
	public string Name { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public string OverviewUrl { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public Func<int, TModel, Task<HttpResponseMessage>> ApiCall { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public TModel Model { get; set; } = default!;

	[Parameter]
	[EditorRequired]
	public int Id { get; set; }

	[Parameter]
	public RenderFragment ChildContent { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public Func<AdminEdit<TModel>, Task> OnPopulate { get; set; } = null!;

	public ErrorState State { get; set; }
	public string? ErrorMessage { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await OnPopulate(this);

		StateHasChanged();
	}

	private async Task OnValidSubmit()
	{
		_submitting = true;

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

		_submitting = false;
	}

	private void Dismiss()
	{
		State = ErrorState.None;
		StateHasChanged();
	}
}
