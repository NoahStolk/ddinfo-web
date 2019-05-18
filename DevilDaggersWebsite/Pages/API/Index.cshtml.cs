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
		public List<ApiFunction> ApiFunctions = new List<ApiFunction>();
		public List<ApiFunction> ApiFunctionsDeprecated = new List<ApiFunction>();

		public void OnGet()
		{
			Assembly asm = AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.FullName.Contains("DevilDaggersWebsite"))
				.FirstOrDefault();

			foreach (Type type in asm.GetTypes())
			{
				if (type.BaseType == typeof(ApiPageModel) && type.Namespace.Contains("API") && !type.Name.Contains("Index"))
				{
					string name = type.Name.Replace("Model", "");

					MethodInfo[] onGets = type.GetMethods().Where(t => t.Name == "OnGet" || t.Name == "OnGetAsync").ToArray();
					foreach (MethodInfo onGet in onGets)
					{
						if (onGet != null)
						{
							ApiFunctionAttribute apiAttribute = (ApiFunctionAttribute)type.GetCustomAttributes(typeof(ApiFunctionAttribute), true).FirstOrDefault();
							if (apiAttribute.IsDeprecated)
								ApiFunctionsDeprecated.Add(new ApiFunction(apiAttribute, name, onGet.GetParameters()));
							else
								ApiFunctions.Add(new ApiFunction(apiAttribute, name, onGet.GetParameters()));
						}
					}
				}
			}
		}
	}
}