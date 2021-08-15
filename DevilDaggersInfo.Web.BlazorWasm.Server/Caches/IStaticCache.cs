namespace DevilDaggersInfo.Web.BlazorWasm.Server.Caches;

public interface IStaticCache : ICache
{
	Task Initiate();
}
