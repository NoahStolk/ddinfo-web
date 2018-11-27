using System.Collections.Generic;
using System.Reflection;

namespace DevilDaggersWebsite.Models.API
{
	public class ApiFunction
	{
		public string Name { get; set; }
		public string ReturnType { get; set; }
		public ParameterInfo[] Parameters { get; set; }

		public ApiFunction(string name, string returnType, params ParameterInfo[] parameters)
		{
			Name = name;
			ReturnType = returnType;
			Parameters = parameters;
		}
	}
}