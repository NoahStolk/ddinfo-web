using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches
{
	public interface ICache
	{
		Task Clear(IWebHostEnvironment env);
	}
}
