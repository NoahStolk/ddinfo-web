using Microsoft.AspNetCore.Hosting;

namespace DevilDaggersWebsite.Caches
{
	public interface ICache
	{
		string LogState(IWebHostEnvironment env);
	}
}
