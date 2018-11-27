using DevilDaggersWebsite.Models.API;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Pages.API
{
	public class IndexModel : PageModel
	{
		public List<ApiFunction> ApiFunction = new List<ApiFunction>();

		public void OnGet()
		{
			Assembly asm = AppDomain.CurrentDomain.GetAssemblies()
				.Where(a => a.FullName.Contains("DevilDaggersWebsite"))
				.FirstOrDefault();

			foreach (Type type in asm.GetTypes())
			{
				if (type.BaseType == typeof(PageModel) && type.Namespace.Contains("API") && !type.Name.Contains("Index"))
				{
					string name = type.Name.Replace("Model", "");

					MethodInfo[] onGets = type.GetMethods().Where(t => t.Name == "OnGet" || t.Name == "OnGetAsync").ToArray();
					foreach (MethodInfo onGet in onGets)
					{
						if (onGet != null)
						{
							ApiAttribute apiAttribute = (ApiAttribute)type.GetCustomAttributes(typeof(ApiAttribute), true).FirstOrDefault();

							ApiFunction.Add(new ApiFunction(name, apiAttribute.ApiReturnType, onGet.GetParameters()));
						}
					}
				}
			}
		}
	}
}