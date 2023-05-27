namespace DevilDaggersInfo.Web.Client.Core.Canvas;

public abstract class Canvas
{
	protected Canvas(string id)
	{
		Id = id;
	}

	protected string Id { get; }
}
