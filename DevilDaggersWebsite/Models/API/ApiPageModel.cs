using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace DevilDaggersWebsite.Models.API
{
	public abstract class ApiPageModel : PageModel
	{
		public FileResult JsonFile(object value, Formatting formatting = Formatting.None)
		{
			string jsonResult = JsonConvert.SerializeObject(value, formatting);
			return File(Encoding.UTF8.GetBytes(jsonResult), MediaTypeNames.Application.Json, $"{GetType().Name.Replace("Model", "")}.json");
		}

		public FileResult JsonFile(object value, string fileName, Formatting formatting = Formatting.None)
		{
			string jsonResult = JsonConvert.SerializeObject(value, formatting);
			return File(Encoding.UTF8.GetBytes(jsonResult), MediaTypeNames.Application.Json, $"{fileName}.json");
		}
	}
}