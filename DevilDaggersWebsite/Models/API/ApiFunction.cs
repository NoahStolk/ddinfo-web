namespace DevilDaggersWebsite.Models.API
{
	public class ApiFunction
	{
		public string Name { get; set; }
		public string ReturnType { get; set; }
		public string[] Parameters { get; set; }

		public ApiFunction(string name, string returnType, params string[] parameters)
		{
			Name = name;
			ReturnType = returnType;
			Parameters = parameters;
		}
	}
}