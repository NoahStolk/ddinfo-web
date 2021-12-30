using DevilDaggersInfo.Web.BlazorWasm.Client.Extensions;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.BlazorWasm.Client.Components.Admin;

public partial class Deletion
{
	public DeleteState State { get; set; }
	public int? IdToDelete { get; set; }
	public string? ApiResponse { get; set; }

	[Parameter] public Func<Task>? AfterDelete { get; set; }
	[Parameter, EditorRequired] public Func<int, Task<HttpResponseMessage>> ApiCall { get; set; } = null!;

	public void Set(int id)
	{
		State = DeleteState.Confirm;
		IdToDelete = id;
	}

	private async Task ConfirmDelete()
	{
		try
		{
			if (!IdToDelete.HasValue)
				return;

			HttpResponseMessage hrm = await ApiCall.Invoke(IdToDelete.Value);
			if (hrm.StatusCode == HttpStatusCode.OK)
			{
				State = DeleteState.Ok;

				if (AfterDelete != null)
					await AfterDelete.Invoke();
			}
			else
			{
				State = hrm.StatusCode.GetDeleteState();
				ApiResponse = await hrm.Content.ReadAsStringAsync();
			}
		}
		catch (AccessTokenNotAvailableException exception)
		{
			exception.Redirect();
		}
	}

	public void Cancel()
	{
		State = DeleteState.None;
		IdToDelete = default;
		StateHasChanged();
	}
}
