using DevilDaggersInfo.Web.Client.Enums;
using DevilDaggersInfo.Web.Client.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace DevilDaggersInfo.Web.Client.Components.Admin;

public partial class AdminEdit<TModel>
{
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
	[EditorRequired]
	public RenderFragment ChildContent { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public Func<AdminEdit<TModel>, Task> OnPopulate { get; set; } = null!;

	public ErrorState State { get; set; }
	public string? ErrorMessage { get; set; }

	protected override async Task OnInitializedAsync()
	{
		await OnPopulate(this);
	}

	private async Task OnValidSubmit()
	{
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
