using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches
{
	public interface IStaticCache : ICache
	{
		Task Initiate();
	}
}
