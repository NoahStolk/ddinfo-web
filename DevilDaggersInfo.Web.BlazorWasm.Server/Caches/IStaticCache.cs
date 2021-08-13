using System.Threading.Tasks;

namespace DevilDaggersWebsite.BlazorWasm.Server.Caches
{
	public interface IStaticCache : ICache
	{
		Task Initiate();
	}
}
