namespace DevilDaggersInfo.Web.Shared.Utils;

public static class PagingUtils
{
	public static int GetValidPageSize(int value)
		=> value < PagingConstants.PageSizeMin || value > PagingConstants.PageSizeMax ? PagingConstants.PageSizeDefault : value;
}
