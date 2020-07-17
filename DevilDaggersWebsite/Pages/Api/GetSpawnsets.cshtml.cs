using DevilDaggersWebsite.Code.Api;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Mime;

namespace DevilDaggersWebsite.Pages.Api
{
	[ApiFunction(Description = "Returns the list of available spawnsets on the site. Optional filtering can be done using the searchAuthor and searchName parameters.", ReturnType = MediaTypeNames.Application.Json)]
	public class GetSpawnsetsModel : ApiPageModel
	{
		private readonly IWebHostEnvironment env;

		public GetSpawnsetsModel(IWebHostEnvironment env)
		{
			this.env = env;
		}

		public FileResult OnGet(string searchAuthor = "", string searchName = "", bool formatted = false) => JsonFile(ApiFunctions.GetSpawnsets(env, searchAuthor, searchName), formatted ? Formatting.Indented : Formatting.None);
	}
}