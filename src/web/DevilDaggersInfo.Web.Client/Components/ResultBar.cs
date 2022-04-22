using DevilDaggersInfo.Web.Client.Enums;
using Microsoft.AspNetCore.Components;

namespace DevilDaggersInfo.Web.Client.Components;

public partial class ResultBar
{
	[Parameter]
	[EditorRequired]
	public RenderFragment ChildContent { get; set; } = null!;

	[Parameter]
	[EditorRequired]
	public ResultBarType ResultBarType { get; set; }

	[Parameter]
	public string? Title { get; set; }

	[Parameter]
	public Action? DismissEvent { get; set; }

	[Parameter]
	public EventCallback<string?> MessageChanged { get; set; }

	public string BackgroundColorClass => ResultBarType switch
	{
		ResultBarType.Success => "bg-darker-green",
		ResultBarType.ValidationError => "bg-darker-orange",
		ResultBarType.FatalError => "bg-darker-red",
		_ => string.Empty,
	};

	public string ButtonColorClass => ResultBarType switch
	{
		ResultBarType.Success => "btn-green",
		ResultBarType.ValidationError => "btn-orange",
		ResultBarType.FatalError => "btn-red",
		_ => string.Empty,
	};
}
