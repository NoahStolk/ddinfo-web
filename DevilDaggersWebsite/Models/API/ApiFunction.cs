using System.Reflection;

namespace DevilDaggersWebsite.Models.API
{
	public class ApiFunction
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string ReturnType { get; set; }
		public ParameterInfo[] Parameters { get; set; }

		public ApiFunction(string name, string description, string returnType, params ParameterInfo[] parameters)
		{
			Name = name;
			Description = description;
			ReturnType = returnType;
			Parameters = parameters;
		}
	}
}