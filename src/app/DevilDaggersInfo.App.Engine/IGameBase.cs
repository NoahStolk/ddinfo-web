namespace Warp.NET;

public interface IGameBase<TSelf>
	where TSelf : IGameBase<TSelf>
{
	static abstract TSelf Self { get; set; }

	static abstract TSelf Construct();
}
