using DevilDaggersWebsite.Code.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Pages
{
	public class ApiModel : PageModel
	{
		public List<Endpoint> Endpoints { get; private set; } = new List<Endpoint>();

		public void OnGet()
		{
			Assembly siteAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("DevilDaggersWebsite"));

			foreach (Type controllerType in siteAssembly.GetTypes().Where(t => t.BaseType == typeof(ControllerBase)))
			{
				string controllerUrl = controllerType.GetCustomAttribute<RouteAttribute>().Template;

				foreach (MethodInfo endpointMethod in controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => !m.Name.Contains("_")))
				{
					HttpMethodAttribute httpMethodAttribute = endpointMethod.GetCustomAttribute<HttpMethodAttribute>(true);
					IEnumerable<ProducesResponseTypeAttribute> responseTypeAttributes = endpointMethod.GetCustomAttributes<ProducesResponseTypeAttribute>();

					if (httpMethodAttribute == null || responseTypeAttributes == null || !responseTypeAttributes.Any())
						continue;

					if (httpMethodAttribute?.Template?.Contains("GetCustomLeaderboards") ?? false)
						Debugger.Break();

					ObsoleteAttribute obsoleteAttribute = endpointMethod.GetCustomAttribute<ObsoleteAttribute>();

					Type returnType = endpointMethod.ReturnType;
					while (returnType.IsGenericType && (returnType.GetGenericTypeDefinition() == typeof(Task<>) || returnType.GetGenericTypeDefinition() == typeof(ActionResult<>)))
						returnType = returnType.GetGenericArguments()[0];

					Endpoints.Add(new Endpoint(
						url: $"{controllerUrl}/{httpMethodAttribute.Template ?? ""}",
						returnType: returnType,
						newRouteToUse: obsoleteAttribute?.Message ?? string.Empty,
						parameters: endpointMethod.GetParameters().Select(p => new EndpointParameter(p)).ToArray(),
						statusCodes: responseTypeAttributes.Select(prt => prt.StatusCode).ToArray()));
				}
			}
		}
	}
}