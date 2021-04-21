using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches
{
	public interface IDynamicCache
	{
		Task Clear(IWebHostEnvironment env);
	}
}
