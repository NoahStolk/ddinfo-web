using DevilDaggersInfo.Web.ApiSpec.Admin;

namespace DevilDaggersInfo.Web.Client.Components.Admin;

public record AdminOverviewColumn<TGetDto, TSorting>
	where TGetDto : IAdminOverviewGetDto
	where TSorting : struct, Enum
{
	public AdminOverviewColumn(TSorting sorting, string header, Func<TGetDto, object?> dataGetter, TextAlign textAlign)
	{
		Sorting = sorting;
		Header = header;
		DataGetter = dataGetter;
		TextAlign = textAlign;
	}

	public TSorting Sorting { get; }

	public string Header { get; }

	public Func<TGetDto, object?> DataGetter { get; }

	public TextAlign TextAlign { get; }

	public string TextAlignCssClass => TextAlign switch
	{
		TextAlign.Right => "text-right",
		_ => "text-left",
	};
}
