namespace DevilDaggersInfo.Web.Client.Utils;

public static class PagingUtils
{
	public static int GetValidPageSize(int value)
	{
		return value < Constants.PageSizeMin || value > Constants.PageSizeMax ? Constants.PageSizeDefault : value;
	}
}
