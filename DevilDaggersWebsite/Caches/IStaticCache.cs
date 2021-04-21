using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches
{
	public interface IStaticCache
	{
		Task Refresh(IWebHostEnvironment env);
	}
}
