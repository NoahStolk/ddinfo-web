using DevilDaggersWebsite.Code.API;
using DevilDaggersWebsite.Code.PageModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Pages.API
{
	public class IndexModel : PageModel
	{
		public List<ApiFunction> ApiFunctions { get; private set; } = new List<ApiFunction>();

		public void OnGet()
		{
			Assembly asm = AppDomain.CurrentDomain.GetAssemblies()
				.FirstOrDefault(a => a.FullName.Contains("DevilDaggersWebsite"));

			foreach (Type type in asm.GetTypes())
				if (type.BaseType == typeof(ApiPageModel) && type.Namespace.Contains("API") && !type.Name.Contains("Index"))
					foreach (MethodInfo onGet in type.GetMethods().Where(t => t.Name == "OnGet" || t.Name == "OnGetAsync").ToArray())
						if (onGet != null)
							ApiFunctions.Add(new ApiFunction(
								(ApiFunctionAttribute)type.GetCustomAttributes(typeof(ApiFunctionAttribute), true).FirstOrDefault(),
								type.Name.Replace("Model", ""),
								onGet.GetParameters().Select(p => new ApiFunctionParameter(p)).ToArray()));
		}
	}
}