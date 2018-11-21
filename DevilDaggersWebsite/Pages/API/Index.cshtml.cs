using DevilDaggersWebsite.Code;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Pages.API
{
	public class IndexModel : PageModel
	{
		public List<ApiPage> ApiPages = new List<ApiPage>();

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

					MethodInfo onGet = type.GetMethod("OnGet");
					if (onGet != null)
					{
						ParameterInfo[] parameterInfos = onGet.GetParameters();

						string[] parameters = new string[parameterInfos.Length];
						for (int i = 0; i < parameterInfos.Length; i++)
							parameters[i] = parameterInfos[i].Name;
						ApiPages.Add(new ApiPage(name, parameters.ToArray()));
					}
					else
					{
						ApiPages.Add(new ApiPage(name));
					}
				}
			}
		}
	}
}