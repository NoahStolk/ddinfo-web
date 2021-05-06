using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches
{
	public interface IDynamicCache : ICache
	{
		Task Clear(IWebHostEnvironment env);
	}
}
